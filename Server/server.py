import cherrypy
from UnityARDatabase import UnityARDatabase

class ARDatabase(object):		
	def __init__(self):
		self.dbcon = UnityARDatabase()
		
	@cherrypy.expose
	def index(self):
		return self.dbcon.getAllDataInHTML()	

	@cherrypy.expose
	def insertToDB(self, scene, str_id, state, time):
		self.dbcon.insertObject(scene, str_id, state, time, cherrypy.request.remote.ip)
		return ""
	
	@cherrypy.expose
	def getFromDB(self, str_id):
		return self.dbcon.getDataForID(str_id)
	
# 	mappings = [(r'^/([^/]+)$', index), 
# 			(r'^/insertToDB/(\d+)$', insertToDB),
# 			(r'^/getFromDB/(\d+)$', getFromDB)]
	
controller = ARDatabase()

cherrypy.config.update({'server.socket_host': '0.0.0.0','server.socket_port': 80}) 
cherrypy.engine.subscribe('start_thread', controller.dbcon.connect) 
cherrypy.quickstart(controller)