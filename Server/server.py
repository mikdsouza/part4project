import cherrypy
import sqlite3
import os
	
class ARDatabase(object):		
	def __init__(self):
		self.dbcon = DBController()
		
	def index(self):
		return self.dbcon.getAllDataInHTML()	
	index.exposed = True
	
	
	def insertToDB(self, str_id, state, time):
		dbcon.insertIntoDB(str_id, state, time, cherrypy.request.remote.ip)
		return str_id
	insertToDB.exposed = True
	
	mappings = [(r'^/([^/]+)$', index), (r'^/insertToDB/(\d+)$', insertToDB)]
	
class DBController:
	
	def __init__(self):
		#Setting up the database
		self.db_filename = 'objs.db'
		
		#Create if db doesn't exist
		if not os.path.exists(self.db_filename):
			print "Database doesn't exist. Creating it"
			self.setUpDB(self.db_filename)
		
	def setUpDB(self, db_filename):
		conn = sqlite3.connect(db_filename)
		c = conn.cursor()
		
		#set up the data table
		c.execute('''
			CREATE TABLE objects (
				id text primary key, 
				state integer default 0, 
				time real, 
				ip text);
			''')
			
		conn.commit()
		conn.close()
	
	def getAllDataInHTML(self):
		conn = sqlite3.connect(self.db_filename)
		c = conn.cursor()
		c.execute('''
			SELECT id, state, time, ip
			FROM objects
			ORDER BY id
			''')
		
		returnString =  "<table border=\"1\" cellpadding=\"2\">\n"
		returnString += "<thead>\n"
		returnString += "<tr>\n"
		returnString += "<td>ID</td>\n"
		returnString += "<td>State</td>\n"
		returnString += "<td>Time</td>\n"
		returnString += "<td>IP address</td>\n"
		returnString += "</tr></thead>\n"
		returnString += "<tbody>\n"
		
		for row in c.fetchall():
			str_id, state, time, ip = row
			returnString += "<tr>\n"
			returnString += "<td>%s</td> <td>%d</td> <td>%f</td> <td>%s</td>" % (str_id, state, time, ip)
			returnString += "</tr>\n"
		
		returnString += "</tbody></table>\n"
		conn.close()
		
		return returnString
	
	def insertIntoDB(self, str_id, state, time, ip):
		conn = sqlite3.connect(self.db_filename)
		c = conn.cursor()
		c.execute('''
			INSERT INTO objects (id, state, time, ip)
			VALUES(\"%s\", %s, %s, \"%s\")
			''' % (str_id, state, time, ip))
		conn.commit()
		conn.close()

dbcon = DBController()
print dbcon.getAllDataInHTML()

cherrypy.config.update({'server.socket_host': '127.0.0.1','server.socket_port': 80}) 
cherrypy.quickstart(ARDatabase())