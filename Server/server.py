import cherrypy
from models import *
from peewee import DoesNotExist
from cherrypy_mako import *
import os

class ARDatabase(object):		
	def __init__(self):
		create_db()
		
	############### ADMIN INTERFACE ###############
	@cherrypy.expose
	@cherrypy.tools.mako(filename="index.html")
	def index(self):
		return {'scenes': Scene.select()}
		
	@cherrypy.expose
	@cherrypy.tools.mako(filename="scene.html")
	def scene(self, id):
		scene = Scene.get(Scene.id == id)
		return {'scene': scene, 'objects': scene.objects.select()}

	############### UNITY INTERFACE ###############
	@cherrypy.expose
	def insertToDB(self, data):
		for object in data.split(";")[:-1]:
			params = object.split(",")
			insertObject(params[0], params[1], params[2], params[3])
			
		return ""
	
	@cherrypy.expose
	def getFromDB(self, scene_name):
		scene = Scene.get(Scene.name == scene_name)
		data = ""
		for object in scene.objects:
			data += "%s,%d,%f;" % (object.identifier, object.state, object.time)

		return data[:-1]
	
	
	
print(os.path.dirname(os.path.realpath(__file__)) + '\static')
cherrypy.config.update({
	'server.socket_host': '0.0.0.0',
	'server.socket_port': 80, 
	'tools.mako.collection_size': 500, 
	'tools.mako.directories': 'templates',
	'tools.staticdir.on' : True,
	'tools.staticdir.dir' : os.path.dirname(os.path.realpath(__file__)) + '\static'
}) 
cherrypy.quickstart(ARDatabase())