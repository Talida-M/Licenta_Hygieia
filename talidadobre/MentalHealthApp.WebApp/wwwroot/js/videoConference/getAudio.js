const connection = new signalR.HubConnectionBuilder().withUrl("/WebRTCHub").build();

const configuration = {
    'iceServers': [
        {
            'urls': 'stun:stun.l.google.com:19302'
        }
    ]
};

const lc = new RTCPeerConnection(configuration);

lc.onicecandidate = (e) => console.log("New Ice candidate, reprinting SDP" + JSON.stringify(lc.localDescription));
const audio = document.getElementById("audio-local");
const audioConstraints = navigator.mediaDevices.getUserMedia({
    audio: true,
});

function getLocalAudio(stream) {
    const audioTrack = stream.getAudioTracks();
    console.log("Device-ul video gasit: " + audioTrack[0].label);  //audioTrack[0] ia primul audio track gasit in stream
    stream.oninactive = function () {
        console.log("Stream ended");
    };
    window.stream = stream; //stream va fi disponibil in consola
    audio.srcObject = stream;//stream este asociat unui tag html <audio>
}
function handleLocalMediaStreamError(error) {
    console.log('navigator.getUserMedia error: ', error);
}
audioConstraints.then(getLocalAudio).catch(handleLocalMediaStreamError);

//////////////////////
const video = document.getElementById("video-local");
const videoconstraints = navigator.mediaDevices.getUserMedia({
    audio: false,
    video: true
});

function getlocalvideo(stream) {
    const videotrack = stream.getVideoTracks();
    console.log("device-ul video gasit: " + videotrack[0].label);  //audiotrack[0] ia primul audio track gasit in stream
    stream.oninactive = function () {
        console.log("stream ended");
    };
    window.stream = stream; //stream va fi disponibil in consola
    video.srcObject = stream;//stream este asociat unui tag html <audio>
}

/////////
function CreatePTPConnection() {
    lc.createOffer().then(e => lc.setLocalDescription(e)).then(a => console.log("Set successfully"));
}

function RemoteConnectionMessage() {
    lc.setRemoteDescription(offer).then(a => console.log("offer set"));
    lc.createAnswer().then(a => rc.setLocalDescription(a)).then(a => console.log("answer created"));
}

/// after setRemoteDescription we have addIceCandidates
async function init(e) {
    try {
        getlocalvideo(videoconstraints);
        e.target.disabled = true;
    }
    catch {
        handleLocalMediaStreamError(e);
    }   

}
button = document.getElementById("showVideo");
function OpenVideo(e) {  init(e) }
