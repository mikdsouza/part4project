import cherrypy
from models import *
from peewee import DoesNotExist
from cherrypy_mako import *

class ARDatabase(object):		
	def __init__(self):
		create_db()
		
	@cherrypy.expose
	@cherrypy.tools.mako(filename="index.html")
	def index(self):
		#return self.dbcon.getAllDataInHTML()
		return {'objects': Object.select()}

	@cherrypy.expose
	def insertToDB(self, scene_name, str_id, state, time):
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
				scene = scene,
				state = int(state) == 1,
				ip = cherrypy.request.remote.ip,
				time = time
			)
			
		return ""
	
	@cherrypy.expose
	def getFromDB(self, str_id):
		object = Object.get(Object.identifier == str_id)
		return "%d,%f" % (object.state, object.time)
	
cherrypy.config.update({'server.socket_host': '0.0.0.0','server.socket_port': 80, 'tools.mako.collection_size': 500, 'tools.mako.directories': 'templates'}) 
cherrypy.quickstart(ARDatabase())