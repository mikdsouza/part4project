from peewee import *

database = SqliteDatabase("unityar.db", threadlocals=True)

class BaseModel(Model):
    class Meta:
        database = database
		
class Scene(BaseModel):
	name = CharField()
	
	def getObjectCount(self):
		return self.objects.count()
	
	def getCheckedObjectCount(self):
		return self.objects.select(Object.state == True).count()

class Object(BaseModel):
	identifier = CharField()
	scene = ForeignKeyField(Scene, related_name='objects')
	state = BooleanField()
	time = FloatField()
	ip = CharField()
	
# Creates the database if it does not already exist
def create_db():
	Scene.create_table(True)
	Object.create_table(True)
	
	# Extra scenes that we want to handle need to go in here, otherwise we get concurrency issues
	scene = Scene.get_or_create(name = 'MultiMarker-Scene')
	scene = Scene.get_or_create(name = 'ObjectCount-Scene')
	scene = Scene.get_or_create(name = 'Axes-Scene')