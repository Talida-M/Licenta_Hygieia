const microphoneBtnOnSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-mic" viewBox="0 0 16 16">
                                <path d="M3.5 6.5A.5.5 0 0 1 4 7v1a4 4 0 0 0 8 0V7a.5.5 0 0 1 1 0v1a5 5 0 0 1-4.5 4.975V15h3a.5.5 0 0 1 0 1h-7a.5.5 0 0 1 0-1h3v-2.025A5 5 0 0 1 3 8V7a.5.5 0 0 1 .5-.5z" />
                                <path d="M10 8a2 2 0 1 1-4 0V3a2 2 0 1 1 4 0v5zM8 0a3 3 0 0 0-3 3v5a3 3 0 0 0 6 0V3a3 3 0 0 0-3-3z" />
                           </svg >`;
const microphoneBtnOffSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-mic-mute" viewBox="0 0 16 16">
                                  <path d="M13 8c0 .564-.094 1.107-.266 1.613l-.814-.814A4.02 4.02 0 0 0 12 8V7a.5.5 0 0 1 1 0v1zm-5 4c.818 0 1.578-.245 2.212-.667l.718.719a4.973 4.973 0 0 1-2.43.923V15h3a.5.5 0 0 1 0 1h-7a.5.5 0 0 1 0-1h3v-2.025A5 5 0 0 1 3 8V7a.5.5 0 0 1 1 0v1a4 4 0 0 0 4 4zm3-9v4.879l-1-1V3a2 2 0 0 0-3.997-.118l-.845-.845A3.001 3.001 0 0 1 11 3z"/>
                                  <path d="m9.486 10.607-.748-.748A2 2 0 0 1 6 8v-.878l-1-1V8a3 3 0 0 0 4.486 2.607zm-7.84-9.253 12 12 .708-.708-12-12-.708.708z"/>
                               </svg>`;
const videoBtnOnSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-camera-video" viewBox="0 0 16 16">
                         <path fill-rule="evenodd" d="M0 5a2 2 0 0 1 2-2h7.5a2 2 0 0 1 1.983 1.738l3.11-1.382A1 1 0 0 1 16 4.269v7.462a1 1 0 0 1-1.406.913l-3.111-1.382A2 2 0 0 1 9.5 13H2a2 2 0 0 1-2-2V5zm11.5 5.175 3.5 1.556V4.269l-3.5 1.556v4.35zM2 4a1 1 0 0 0-1 1v6a1 1 0 0 0 1 1h7.5a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1H2z" />
                      </svg>`;
const videoBtnOffSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-camera-video-off" viewBox="0 0 16 16">
                          <path fill-rule="evenodd" d="M10.961 12.365a1.99 1.99 0 0 0 .522-1.103l3.11 1.382A1 1 0 0 0 16 11.731V4.269a1 1 0 0 0-1.406-.913l-3.111 1.382A2 2 0 0 0 9.5 3H4.272l.714 1H9.5a1 1 0 0 1 1 1v6a1 1 0 0 1-.144.518l.605.847zM1.428 4.18A.999.999 0 0 0 1 5v6a1 1 0 0 0 1 1h5.014l.714 1H2a2 2 0 0 1-2-2V5c0-.675.334-1.272.847-1.634l.58.814zM15 11.73l-3.5-1.555v-4.35L15 4.269v7.462zm-4.407 3.56-10-14 .814-.58 10 14-.814.58z"/>
                        </svg>`;
const connection = new signalR.HubConnectionBuilder().withUrl("/WebRTCHub").build();
//const topic = "Consultatie1"; // generare tot de pe server

console.log('Room id : ' + myRoomId);


const configuration = {
    'iceServers': [
        {
            'urls': 'stun:stun1.l.google.com:19302'
        }
    ]
};

const localConnection = new RTCPeerConnection(configuration);
grabWebCamVideo();

localConnection.onicecandidate = (event) => {
    console.log('ice candidate event triggered');
    if (event.candidate) {
        sendMessage({ 'candidate': event.candidate });
    }
};

localConnection.onconnectionstatechange = (event) => {
    console.log('connection state change event triggered');
    if (localConnection.connectionState === 'connected') {
        remoteVideoCard = addVideoStream(remoteStream, false);
        videoGrid.appendChild(remoteVideoCard);
        console.log("peers connected");
    }
    else if (localConnection.connectionState == 'disconnected') {
        remoteVideoCard.remove();
        console.log('peers disconnected');
    }
};

localConnection.ontrack = async (event) => {
    console.log('received remote track');
    remoteStream.addTrack(event.track, remoteStream);
};

const videoGrid = document.getElementById('Dish');

const screenShareBtn = document.getElementById('btn-screenshare');
const microphoneBtn = document.getElementById('btn-microphone');
const videoBtn = document.getElementById('btn-video');
screenShareBtn.setAttribute("state", "off");
microphoneBtn.setAttribute("state", "on");
videoBtn.setAttribute("state", "on");
microphoneBtn.innerHTML = microphoneBtnOnSvg;
videoBtn.innerHTML = videoBtnOnSvg;

const senders = [];
let displayMediaStream = null;
let userMediaStream = null;
let localVideoCard = null;
let remoteVideoCard = null;
let localStream = null;
let remoteStream = new MediaStream();

const isInitiatorFlag = (isInitiator == 1);

function grabWebCamVideo() {
    navigator.mediaDevices.getUserMedia({
        audio: true,
        video: true
    })
        .then(gotStream)
        .catch(function (e) {
        });
}

function gotStream(stream) {
    userMediaStream = stream;
    localStream = stream;
    localVideoCard = addVideoStream(localStream, true);
    localStream.getTracks().forEach(track => {
        senders.push(localConnection.addTrack(track, localStream));
    });
}

function addVideoStream(stream, isLocalStream) {
    let videoContainer = document.createElement('div');
    let videoStream = document.createElement('video');
    videoContainer.className = 'Camera';
    videoContainer.appendChild(videoStream);
    videoStream.srcObject = stream;

    if (isLocalStream) {
        videoStream.muted = true;
    }

    videoStream.addEventListener('loadedmetadata', () => {
        videoStream.play();
    });

    videoGrid.appendChild(videoContainer);
    //call this to load css on appended element
    Dish();
    return videoContainer;
}

connection.start().then(function () {

    connection.on('message', async (message) => {
        console.log('msg received : ' + message);
        signalingMessageCallback(message);
    });

    connection.on('joinedRoom', (uuid) => {
        console.log('joined room ' + uuid);

        // the callee needs some time to add his localStream tracks to his RTCPeerConnection obj
        setTimeout(() => {
            createPeerConnection();
        }, 4000);
    });

    connection.on('leftRoom', () => {
        remoteVideoCard.remove();
        console.log('peer left');
    });

    connection.invoke('Join', (myRoomId)).catch(function (err) {
        return console.error(err.toString());
    });

    window.onbeforeunload = function () {
        connection.invoke("LeaveRoom", myRoomId).catch(function (err) {
            return console.error(err.toString());
        });
    };

}).catch(function (err) {
    return console.error(err.toString());
});

function sendMessage(message) {
    console.log('Client sending message : ' + message);
    connection.invoke("SendMessage", myRoomId, message).catch(function (err) {
        return console.error(err.toString());
    });
}

function createPeerConnection() {
    if (isInitiatorFlag) {
        console.log('Creating an offer');
        localConnection.createOffer()
            .then(offer => {
                localConnection.setLocalDescription(offer);
                sendMessage({ 'offer': offer });
            });
    }
    else {
        console.log('Waiting for an offer');
    };
}

async function signalingMessageCallback(message) {
    console.log(message);

    if (message.offer) {
        console.log('offer received');
        localConnection.setRemoteDescription(new RTCSessionDescription(message.offer));
        const answer = await localConnection.createAnswer();
        await localConnection.setLocalDescription(answer);
        sendMessage({ 'answer': answer });
    }

    if (message.answer) {
        console.log('answer received');
        const remoteDesc = new RTCSessionDescription(message.answer);
        await localConnection.setRemoteDescription(remoteDesc);
    }

    if (message.candidate) {
        try {
            console.log('new ice candidate received');
            await localConnection.addIceCandidate(message.candidate);
        } catch (e) {
            console.error('error adding received ice candidate', e);
        }
    }
}

function logError(err) {
    if (!err) return;
    if (typeof err === 'string') {
        console.warn(err);
    } else {
        console.warn(err.toString(), err);
    }
}

screenShareBtn.onclick = async () => {
    let state = screenShareBtn.getAttribute("state");
    if (state === "off") {
        if (!displayMediaStream) {
            displayMediaStream = await navigator.mediaDevices.getDisplayMedia();
        }
        senders.find(sender => sender.track.kind === "video").replaceTrack(displayMediaStream.getTracks()[0]);
        localStream = displayMediaStream;
        localVideoCard.children[0].srcObject = displayMediaStream;
        screenShareBtn.setAttribute("state", "on");
    }
    else {
        senders.find(sender => sender.track.kind === "video")
            .replaceTrack(userMediaStream.getTracks()
                .find(track => track.kind === "video"));
        localStream = userMediaStream;
        localVideoCard.children[0].srcObject = userMediaStream;
        screenShareBtn.setAttribute("state", "off");
    }
}

microphoneBtn.onclick = async () => {
    let state = microphoneBtn.getAttribute("state");

    audioTrack = senders.find(sender => sender.track.kind === "audio").track;
    audioTrack.enabled = !audioTrack.enabled;

    if (state === "on") {
        microphoneBtn.innerHTML = microphoneBtnOffSvg;
        microphoneBtn.setAttribute("state", "off");
    }
    else {
        microphoneBtn.innerHTML = microphoneBtnOnSvg;
        microphoneBtn.setAttribute("state", "on");
    }
}

videoBtn.onclick = async () => {
    let state = videoBtn.getAttribute("state");

    videoTrack = senders.find(sender => sender.track.kind === "video").track;
    videoTrack.enabled = !videoTrack.enabled;

    if (state === "on") {
        videoBtn.innerHTML = videoBtnOffSvg;
        videoBtn.setAttribute("state", "off");
    }
    else {
        videoBtn.innerHTML = videoBtnOnSvg;
        videoBtn.setAttribute("state", "on");
    }
}