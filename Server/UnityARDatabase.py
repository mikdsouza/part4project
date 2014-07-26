import sqlite3
import os
import threading

class UnityARDatabase:
	
	def __init__(self):
		#Setting up the database
		self.db_filename = 'objs.db'
		self.data=threading.local()
		
		#Create if db doesn't exist
		if not os.path.exists(self.db_filename):
			print("Database doesn't exist. Creating it")
			self.setUpDB()
			
	# This function is called by every thread when it is started
	# Each thread needs to have a separate connection
	def connect(self, thread_index):
		self.data.conn = sqlite3.connect(self.db_filename)
		
	def setUpDB(self):
		conn = sqlite3.connect(self.db_filename)
		c = conn.cursor()
		
		#set up the data table
		c.execute('''
			CREATE TABLE objects (
				id text primary key,
				scene integer,
				state integer default 0, 
				time real, 
				ip text
			);
		''')
		
		# set up table for the different scenes
		c.execute('''
			CREATE TABLE scenes (
				id integer primary key,
				name text
			);
		''')
		
		# Insert scenes
		c.execute('''
			INSERT INTO scenes (name) VALUES ("MultiMarker-Scene")
		''')
			
		conn.commit()
		conn.close()
	
	# TODO move to template system
	def getAllDataInHTML(self):
		c = self.data.conn.cursor()
		
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
		
		return returnString
	
	def insertObject(self, scene, str_id, state, time, ip):
		c = self.data.conn.cursor()
		
		scene = self.getSceneId(scene)
		
		c.execute('''
			INSERT OR REPLACE INTO objects (scene, id, state, time, ip)
			VALUES(%d, \"%s\", %s, %s, \"%s\")
			''' % (scene, str_id, state, time, ip))
		self.data.conn.commit()
	
	def selectObject(self, str_id):
		pass
	
	def getDataForID(self, str_id):
		c = self.data.conn.cursor()
		
		c.execute('''
			SELECT state, time
			FROM objects
			WHERE id = \"%s\"
			''' % (str_id))
		
		returnString = ""
		for row in c.fetchall():
			state, time = row
			returnString = "%d,%f" % (state, time)
		
		return returnString
	
	def getSceneId(self, name):
		c = self.data.conn.cursor()
		
		c.execute('''
			SELECT id FROM scenes WHERE name = "%s"
			''' % name)

		return c.fetchone()[0]