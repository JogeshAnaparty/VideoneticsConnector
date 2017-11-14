
#!/usr/bin/env python
from threading import Lock
from flask import Flask, render_template, session, request, jsonify
from flask_socketio import SocketIO, emit, join_room, leave_room, close_room, rooms, disconnect
import json

async_mode = None
default_namespace='/requests'
default_response_topic ='response'

# API routes
version_route='/version'
channels_list_route='/REST/local/channel/'
channels_status_route='/REST/local/channel/status'
startlive_route='/REST/local/startlive/<channel_id>/<width>/<height>/<audio_enabled>/'
stoplive_route='/REST/local/stoplive/<session_id>'
keepalive_route='/REST/local/live/keepalive/<session_id>'
startarchive_route='/REST/local/startarchive/<channel_id>/<width>/<height>/<unix_timestamp>/<audio_enabled>'
stoparchive_route='/REST/local/stoparchive/<session_id>'
keepalive_archive_route='/REST/local/archive/keepalive/<session_id>'

app = Flask(__name__)
app.config['SECRET_KEY'] = 'secret!'
socketio = SocketIO(app, async_mode=async_mode)

with open('config.json','r') as f:
    config = json.load(f)

@app.route('/')
def index():
    return render_template('index.html', async_mode=socketio.async_mode)

@app.route(version_route)
def get_version():
    response = build_ok_response('./api_responses/version.json')
    return jsonify(response)

@app.route(channels_list_route)
def get_channels_list():
    print('Loading status...')
    response = build_ok_response('./api_responses/channels_list.json', emit=False)
    print('Channels loaded: sending result to the client...')
    return jsonify(response)

@app.route(channels_status_route)
def get_channels_status():
    response = build_ok_response('./api_responses/channels_status.json')
    return jsonify(response)

@app.route(startlive_route)
def startlive(channel_id, width, height, audio_enabled):
    response = build_ok_response('./api_responses/startlive.json', emit=False)
    for item in response: 
        if ('channel_id' in item) and (item['channel_id'] == int(channel_id)):
           socketio.emit(default_response_topic, item,  namespace=default_namespace)   
           return jsonify(item)
        else:
            print('Item not found!')

    return jsonify({"error":500})

@app.route(stoplive_route)
def stoplive(session_id):
    response = build_ok_response('./api_responses/stoplive.json')
    return jsonify(response)

@app.route(keepalive_route)
def keeplive(session_id):
    response = build_ok_response('./api_responses/keepalive.json')
    return jsonify(response)

@app.route(startarchive_route, methods=['GET', 'POST'])
def startarchive(channel_id, width, height,unix_timestamp, audio_enabled):
    response = build_ok_response('./api_responses/startarchive.json', emit=False)
    for item in response: 
        if ('channel_id' in item) and (item['channel_id'] == int(channel_id)):
           socketio.emit(default_response_topic, item,  namespace=default_namespace)   
           return jsonify(item)
        else:
            print('Item not found!')

    return jsonify({"error":500})

@app.route(stoparchive_route)
def stoparchive(session_id):
    response = build_ok_response('./api_responses/stoparchive.json')
    return jsonify(response)

@app.route(keepalive_archive_route)
def keeplive_archive(session_id):
    response = build_ok_response('./api_responses/keepalive_archive.json')
    return jsonify(response)

def build_ok_response(template, emit=True):
    with open(template,'r') as f:
         response = json.load(f)
    if emit is True:
        socketio.emit(default_response_topic, response['ok_response'],  namespace=default_namespace) 

    return response['ok_response']

if __name__ == '__main__':
    socketio.run(app, config['webserver']['host'], config['webserver']['port'],debug=True)
