const connection = new signalR.HubConnectionBuilder().withUrl("/WebRTCHub").build();

const configuration = {
    'iceServers': [
        {
            'urls': 'stun:stun.l.google.com:19302'
        }
    ]
};

function sendToServer(message) {
    const msgJSON = JSON.stringify(message);

    connection.send(msgJSON);
}

let localStream = null;

const peerConnection = new RTCPeerConnection(configuration);

const Constraints = {

    audio: true,
    video: true
};

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


function startCall(event) {
    if (peerConnection) {
        alert("You are already in a call");
    }
    else {
        CreatePTPConnection();
        const userMedia = navigator.mediaDevices.getUserMedia(Constraints);
        const object = document.getElementById("local_video");
        userMedia.then((media) => {
            object.srcObject = media;
            const tracks = media.getTracks();
            const video = tracks.find(t => t.kind === 'video');
            const audio = tracks.find(t => t.kind === 'audio');
            //peerConnection.addTrack(tracks, localStream);
        }); /// aduagam fiecare track gasit in RTCPeerConnection
        //localStream.oninactive = function () {
        //    console.log("Stream ended");
        //};
        window.stream = localStream;
    }
}

function handleGetUserMediaError(e) {
    switch (e.name) {
        case "NotFoundError":
            alert("No camera and/or microphone were found.");
            break;
        case "SecurityError":
        case "PermissionDeniedError":
            // Do nothing; this is the same as the user canceling the call.
            break;
        default:
            alert(`Error opening your camera and/or microphone: ${e.message}`);
            break;
    }

    LeaveAppointment();
}

/// 
function handleICECandidateEvent(event) {
    if (event.candidate) {
        sendToServer({
            type: "new-ice-candidate",
            target: targetUsername,
            candidate: event.candidate
        });
    }
}
//receive ice candidates
function handleNewICECandidateMsg(message) {
    const candidate = new RTCIceCandidate(message.candidate);

    peerConnection.addIceCandidate(candidate);
}
function handleNegotiationNeededEvent() {
    peerConnection.createOffer().then(e => peerConnection.setLocalDescription(e))
        .then(a => console.log("Set successfully"),
                    sendToServer({
                        //name: myUsername,
                        //target: targetUsername,
                        type: "video-offer",
                        sdp: peerConnection.localDescription
                    }));
}
//afiseaza video-ul trimis de catre un alt peer si ofera posibilitatea userului sa inchida apelul
function handleTrackEvent(event) {
    document.getElementById("received_video").srcObject = event.streams[0];
    document.getElementById("hangup-button").disabled = false;
}

function CreatePTPConnection() {
    peerConnection = new RTCPeerConnection(configuration);
    peerConnection.onicecandidate = e => console.log("New Ice candidate, reprinting SDP" + JSON.stringify(lc.localDescription));  
    peerConnection.ontrack = handleTrackEvent; //conecteaza un element media la un element care il va afisa
    peerConnection.onnegotiationneeded = handleNegotiationNeededEvent; // are jobul de crea si trimite oferte, se cereconectarea cu noi
}

function joinVideoOffer(message) {
    targetUsername = message.name;
    CreatePTPConnection();
    peerConnection.setRemoteDescription( new RTCSessionDescription(message.sdp)); // luam sdp offer creat la inceput
    localStream = Constraints;
    object.srcObject = localStream;
    localStream.getTracks().forEach((track) => peerConnection.addTrack(track, localStream)); /// aduagam fiecare track gasit in RTCPeerConnection
    peerConnection.createAnswer().then(e => peerConnection.setLocalDescription(e))
        .then(a => console.log("Set successfully"),
            sendToServer({
                //name: myUsername,
                //target: targetUsername,
                type: "video-offer",
                sdp: peerConnection.localDescription
            }));
}

function LeaveAppointment() {
    const remoteVideo = document.getElementById("received_video");
    const localVideo = document.getElementById("local_video");
    if (peerConnection) {
        //prevenirea declansarii lor in momentul procesului de inchidere a conexiunii
        peerConnection.ontrack = null;
        peerConnection.onicecandidate = null;
        peerConnection.onnegotiationneeded = null;

        if (remoteVideo.srcObject) {
            remoteVideo.srcObject.getTracks().forEach((track) => track.stop());
        }

        if (localVideo.srcObject) {
            localVideo.srcObject.getTracks().forEach((track) => track.stop());
        }

        peerConnection.close();
        peerConnection = null;
    }
    remoteVideo.removeAttribute("src");
    remoteVideo.removeAttribute("srcObject");
    localVideo.removeAttribute("src");
    remoteVideo.removeAttribute("srcObject");

    document.getElementById("hangup-button").disabled = true;
    targetUsername = null;
}
function hangUpCall() {
    LeaveAppointment();
    sendToServer({
        name: myUsername,
        target: targetUsername,
        type: "hang-up"
    });
}