var map;
var marker = false;

function setMarkerPosition(position) {
    if (marker === false) {
        //Create the marker.
        marker = new google.maps.Marker({
            position: position,
            map: map,
            draggable: true
        });
        //Listen for drag events!
        google.maps.event.addListener(marker, "dragend", function (event) {
            updateLocationFromMarker();
        });
    } else {
        marker.setPosition(position);
    }
    updateLocationFromMarker();
}

function initMap() {
    map = new google.maps.Map(document.getElementById("map"),
    {
        center: { lat: 54.6872, lng: 25.2797 },
        zoom: 13,
        clickableIcons: false
    });

    // Create the search box and link it to the UI element.
    var input = document.getElementById('pac-input');
    var searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });

    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }

        var place = places[0];
        if (!place.geometry) {
            console.log("Returned place contains no geometry");
            return;
        }

        setMarkerPosition(place.geometry.location);

        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            if (!place.geometry) {
                console.log("Returned place contains no geometry");
                return;
            }
            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });

    google.maps.event.addListener(map, "click", function (event) {
        setMarkerPosition(event.latLng);
    });
}

function updateLocationFromMarker() {
    var currentLocation = marker.getPosition();
    document.getElementById("latitude").value = currentLocation.lat();
    document.getElementById("longitude").value = currentLocation.lng();
}

var text_max = 693;
$("#count_message").html("0 / " + text_max);

$("#text").keyup(function () {
    var text_length = $("#text").val().length;

    $("#count_message").html(text_length + " / " + text_max);
});