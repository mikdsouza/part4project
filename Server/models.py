from peewee import *
import cherrypy

database = SqliteDatabase("unityar.db", threadlocals=True)

class BaseModel(Model):
    class Meta:
        database = database
		
class Scene(BaseModel):
	name = CharField()
	
	def getObjectCount(self):
		return self.objects.count()
	
	def getCheckedObjectCount(self):
		return self.objects.select().where(Object.state == True).count()

class Object(BaseModel):
	identifier = CharField()
	marker = CharField()
	scene = ForeignKeyField(Scene, related_name='objects')
	state = BooleanField()
	time = FloatField()
	ip = CharField()
	
# Creates the database if it does not already exist
def create_db():
	Scene.create_table(True)
	Object.create_table(True)
	
	# Extra scenes that we want to handle need to go in here, otherwise we get concurrency issues
	Scene.get_or_create(name = 'MultiMarker-Scene')
	Scene.get_or_create(name = 'ObjectCount-Scene')
	Scene.get_or_create(name = 'axes-Scene')
	Scene.get_or_create(name = 'example-scene')
	Scene.get_or_create(name = 'ShowOff-Scene')
	Scene.get_or_create(name = 'HourGlass-Scene')
	Scene.get_or_create(name = 'AutoApplyTest-Scene')
	Scene.get_or_create(name = 'Bouncing-Scene')
	Scene.get_or_create(name = 'House-Scene')
	Scene.get_or_create(name = 'Truss-Scene')
	
def insertObject(scene_name, marker_id, str_id, state, time):
	try:
		object = Object.get(Object.identifier == str_id)
		object.state = int(state) == 1
		object.time = time
		object.ip = cherrypy.request.remote.ip
		object.save()
	
	except DoesNotExist:
		scene = Scene.get_or_create(name = scene_name)
		object = Object.create(
		    identifier = str_id,
			marker = marker_id,
		    scene = scene,
		    state = int(state) == 1,
		    ip = cherrypy.request.remote.ip,
		    time = time
		)