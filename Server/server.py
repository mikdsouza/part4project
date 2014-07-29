import cherrypy
from models import *
from peewee import DoesNotExist
from cherrypy_mako import *
import os
#import os.path

current_directory = os.path.dirname(os.path.realpath(__file__))

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
		objects = scene.objects.select()
		
		screenshots = set()
		for object in objects:
			filename = "%s\static\screenshots\%s-%s.png" % (current_directory, object.scene.name, object.marker)
			if os.path.isfile(filename):
				screenshots.add("screenshots/%s-%s.png" % (object.scene.name, object.marker))
		
		return {'scene': scene, 'objects': objects, 'screenshots': screenshots}

	############### UNITY INTERFACE ###############
	@cherrypy.expose
	def insertToDB(self, data):
		for object in data.split(";")[:-1]:
			params = object.split(",")
			insertObject(params[0], params[1], params[2], params[3], params[4])
			
		return ""
	
	@cherrypy.expose
	def getFromDB(self, scene_name):
		scene = Scene.get(Scene.name == scene_name)
		data = ""
		for object in scene.objects:
			data += "%s,%d,%f;" % (object.identifier, object.state, object.time)

		return data[:-1]
		
	@cherrypy.expose
	def upload(self, file, marker, scene):
		dest = "%s\static\screenshots\%s-%s.png" % (current_directory, scene, marker)
		destFile = open(dest, 'wb')

		while True:
			data = file.file.read(8192)
			if not data:
				break
			destFile.write(data)
		
		return ""
	
print(os.path.dirname(os.path.realpath(__file__)) + '\static')
cherrypy.config.update({
	'server.socket_host': '0.0.0.0',
	'server.socket_port': 80, 
	'tools.mako.collection_size': 500, 
	'tools.mako.directories': 'templates',
	'tools.staticdir.on' : True,
	'tools.staticdir.dir' : current_directory + '\static'
}) 
cherrypy.quickstart(ARDatabase())