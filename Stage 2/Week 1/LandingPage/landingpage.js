    //The following script creates a Google map that marks Glacier National Park
    // Initialize and add the map
    function initMap() {
      // The location of Glacier National Parl
      const glacierNP = { lat: 48.7550, lng: -113.8000 };
      // The map, centered at Glacier National Park
      const map = new google.maps.Map(document.getElementById("mymap"), {
        zoom: 10,
        center: glacierNP,
      });
      // The marker, positioned at Glacier
      const marker = new google.maps.Marker({
        position: glacierNP,
        map: map,
      });
    }