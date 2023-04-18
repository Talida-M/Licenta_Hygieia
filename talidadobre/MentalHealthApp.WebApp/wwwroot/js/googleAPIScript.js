//creating function to load initial MAP when page loading   
function initMap() {

    var markers = [];
    const address = document.getElementById('p-adresa').childNodes[2].data;
    if (window.localStorage.getItem("adresa cabinet:") !== null) {
        window.localStorage.removeItem("adresa cabinet:");
    }
    window.localStorage.setItem("adresa cabinet: ", address);
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            async (position) => {

                const apiKey = 'AIzaSyDoXRvHuYgJDyVIhkYxJN8zhPa_6tRbces';
                const url = `https://maps.googleapis.com/maps/api/geocode/json?key=${apiKey}&address=${address}`;
                const location = await fetch(url)
                    .then(resp => resp.json())
                    .then(data => data.results[0]?.geometry.location);
                let latitude = location !== undefined ? location.lat : position.coords.latitude;
                let longitude = location !== undefined ? location.lng : position.coords.longitude;


                map = new google.maps.Map(
                    document.getElementById("map"),
                    {
                        center: { lat: latitude, lng: longitude },
                        zoom: 15,
                        mapTypeId: google.maps.MapTypeId.ROADMAP,
                        streetViewControl: true,
                    }
                )
                map.setTilt(45);
                marker = new google.maps.Marker({
                    position: { lat: latitude, lng: longitude },
                    map: map,
                    title: 'Your doctor office'
                });

                // Create the search box and link it to the UI element.  
                var input = document.getElementById('txtsearch');
                map.controls[google.maps.ControlPosition.TOP_CENTER].push(input);
                var searchBox = new google.maps.places.SearchBox(input);
      

                //// Listen for the event fired when the user selects an item from the  
                //// pick list. Retrieve the matching places for that item.  
                google.maps.event.addListener(searchBox, 'places_changed', function () {
                    var places = searchBox.getPlaces();

                    if (places.length == 0) {
                        return;
                    }
                    for (var i = 0, marker; marker = markers[i]; i++) {
                        marker.setMap(null);
                    }

                    // For each place, get the icon, place name, and location.  
                    markers = [];
                    var bounds = new google.maps.LatLngBounds();
                    for (var i = 0, place; place = places[i]; i++) {
                        var image = {
                            url: place.icon,
                            size: new google.maps.Size(71, 71),
                            origin: new google.maps.Point(0, 0),
                            anchor: new google.maps.Point(17, 34),
                            scaledSize: new google.maps.Size(25, 25)
                        };

                        //        // Create a marker for each place.  
                        var marker = new google.maps.Marker({
                            map: map,
                            icon: image,
                            title: place.name,
                            position: place.geometry.location
                        });

                        markers.push(marker);

                        bounds.extend(place.geometry.location);
                    }

                    map.fitBounds(bounds);
                });



                //// current map's viewport.  
                google.maps.event.addListener(map, 'bounds_changed', function () {
                    var bounds = map.getBounds();
                    searchBox.setBounds(bounds);
                });
            });
    }
    else {
        map = new google.maps.Map(
            document.getElementById("map"),
            {
                center: { lat: -34.397, lng: 150.644  },
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                streetViewControl: true,
            }
        )
    }
    
}
window.addEventListener("load", initMap);