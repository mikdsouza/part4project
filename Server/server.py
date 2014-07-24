import cherrypy
class HelloWorld(object):
    def index(self):
        return "Hello Mikki!"
    index.exposed = True

cherrypy.quickstart(HelloWorld())