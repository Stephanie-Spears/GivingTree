let map, infoWindow;

function initMap() {
	const myLatLng = { lat: 45.502424816134386, lng: -122.63250256190184 };
	map = new window.google.maps.Map(document.getElementById("map"),
		{ 
			center: myLatLng,
			zoom: 14
		}); 
	
	// Create the initial InfoWindow.
	let infoWindow = new window.google.maps.InfoWindow({ 
		content: "Click the map to get Lat/Lng!",
		position: myLatLng
	});
	infoWindow.open(map);
	
	// Configure the click listener.
	map.addListener("click",
		(mapsMouseEvent) => {
			// Close the current InfoWindow.
			infoWindow.close();
			// Create a new InfoWindow.
			infoWindow = new window.google.maps.InfoWindow({ 
				position: mapsMouseEvent.latLng
			});
			infoWindow.setContent(
				JSON.stringify(mapsMouseEvent.latLng.toJSON(), null, 2)
			);
			infoWindow.open(map);
		});

	const locationButton = document.createElement("button");
	locationButton.textContent = "Pan to Current Location";
	locationButton.classList.add("custom-map-control-button");
	map.controls[window.google.maps.ControlPosition.TOP_CENTER].push(locationButton);
	locationButton.addEventListener("click",
		() => {
			// Try HTML5 geolocation.
			if (navigator.geolocation) {
				navigator.geolocation.getCurrentPosition(
					(position) => {
						const pos = {
							lat: position.coords.latitude,
							lng: position.coords.longitude
						};
						infoWindow.setPosition(pos);
						infoWindow.setContent("Location found.");
						infoWindow.open(map);
						map.setCenter(pos);
					},
					() => {
						handleLocationError(true, infoWindow, map.getCenter());
					}
				);
			} else {
				// Browser doesn't support Geolocation
				handleLocationError(false, infoWindow, map.getCenter());
			}
		});
	
	function handleLocationError(browserHasGeolocation, infoWindow, pos) {
		infoWindow.setPosition(pos);
		infoWindow.setContent(
			browserHasGeolocation
			? "Error: The Geolocation service failed."
			: "Error: Your browser doesn't support geolocation."
		);
		infoWindow.open(map);
	} 
	
	// set types of icons, like identifying tree types by custom fruit markers
	const iconBase = "/Content/Images/Optimized/Compressed/";
	const icons = {
		appleTree: {
			icon: iconBase + "apple.ico"
		},
		cherryTree: {
			icon: iconBase + "cherry.ico"
		},
		figTree: {
			icon: iconBase + "fig.ico"
		},
		figPurpleTree: {
			icon: iconBase + "fig-purple.ico"
		},
		pearTree: {
			icon: iconBase + "pear.ico"
		},
		persimmonTree: {
			icon: iconBase + "persimmon.ico"
		},
		plumTree: {
			icon: iconBase + "plum.ico"
		}
	}; 
	
	// hardcoded icon locations
	const features = [
		{
			position: new window.google.maps.LatLng(45.50896284997959, -122.64174555594482),
			type: "appleTree"
		},
		{ 
			position: new window.google.maps.LatLng(45.519457696849315, -122.65285015869141),
			type: "appleTree"
		},
		{ 
			position: new window.google.maps.LatLng(45.515417214354365, -122.59632492670897),
			type: "cherryTree"
		},
		{ 
			position: new window.google.maps.LatLng(45.50049976134157, -122.60774040827636),
			type: "cherryTree"
		},
		{
			position: new window.google.maps.LatLng(45.50440995995079, -122.61666679987792),
			type: "figTree"
		},
		{
			position: new window.google.maps.LatLng(45.493942049504305, -122.62765312800292),
			type: "figTree"
		},
		{
			position: new window.google.maps.LatLng(45.519157002389406, -122.62791392331543),
			type: "figPurpleTree"
		},
		{
			position: new window.google.maps.LatLng(45.50676699368567, -122.65203234677735),
			type: "figPurpleTree"
		},
		{
			position: new window.google.maps.LatLng(45.52034872832883, -122.60447884211425),
			type: "pearTree"
		},
		{
			position: new window.google.maps.LatLng(45.49117434563012, -122.64250514035645),
			type: "pearTree"
		},
		{
			position: new window.google.maps.LatLng(45.4898506130879, -122.61203194270018),
			type: "persimmonTree"
		},
		{
			position: new window.google.maps.LatLng(45.48359254718654, -122.6207866729248),
			type: "persimmonTree"
		},
		{
			position: new window.google.maps.LatLng(45.49261838201237, -122.59409332880858),
			type: "plumTree"
		},
		{
			position: new window.google.maps.LatLng(45.49941688909526, -122.6561060012329),
			type: "plumTree"
		}
	]; 
	
	// create markers
	for (let i = 0; i < features.length; i++) {
		const marker = new window.google.maps.Marker({
			position: features[i].position,
			icon: icons[features[i].type].icon,
			map: map
		});
	}
}
// todo: add information tooltip on icon click
