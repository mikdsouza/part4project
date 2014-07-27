from peewee import *

database = SqliteDatabase("unityar.db", threadlocals=True)

class BaseModel(Model):
    class Meta:
        database = database
		
class Scene(BaseModel):
    name = CharField()

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