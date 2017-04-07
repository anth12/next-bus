try {
    window.app = {
        busUpdateInterval: -1,
        currentBusStop: null,
        busStops: [],
        busStopLoockupScreen: null,
        arrivingSoonScreen: null,
        notification: null,

        init: function() {

            app.busStopLoockupScreen = document.getElementsByClassName('bus-stop-lookup')[0];
            app.arrivingSoonScreen = document.getElementsByClassName('arriving-soon')[0];
            app.notification = document.getElementsByClassName('notification')[0];
                        
            app.loadStops(function() {

                app.findClosestStops();
            });

            setInterval(function() {

                app.updateDynamicTimes();
            }, 5000);
			
			setTimeout(function(){
				app.app.showClosestStops();
			}, 5000);
        },

        loadStops: function(callback) {

            if (localStorage.length <= 1) {
                // load the stops from the API

                app._showNotification('Loading bus stops');

                fetch('/BusApi/GetStops').then(function(response) {
                    // Convert to JSON
                    return response.json();
                }).then(function(data) {

                    app.busStops = data;

                    // Save the data in localStorage
                    data.forEach(function(busStop) {
                        localStorage.setItem(busStop.Id, JSON.stringify(busStop));
                    });

                    app._hideNotification();

                    callback();
                });

            } else {
                // Load the data from localStorage
                for (var key in localStorage) {
                    var busStop = JSON.parse(localStorage.getItem(key));

                    app.busStops.push(busStop);
                }

                callback();
            }
        },

        configureSearch: function() {
            var searchResults = document.getElementsByClassName('searched-stops')[0];

            var html = '';
            app.busStops.forEach(function(busStop) {

                html += app._busStopTemplate(busStop, true);
            });

            searchResults.innerHTML = html;

            // once the DOM elements are rendered, attach the even handlers
            setTimeout(function() {
                document.querySelectorAll('.bus-stop-lookup ul li').forEach(function(elm) {
                    elm.addEventListener('click', app.selectBusStop);
                });
            }, 50);

        },

        searchStops: function() {
            var searchTerm = document.getElementsByClassName('search')[0].value;

            document.querySelectorAll('.searched-stops li').forEach(function(item) {
                var stopName = item.dataset.name;

                if (stopName.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1) {
                    // Show
                    item.className = "";
                } else {
                    // Hide
                    item.className = "hidden";
                }
            });
        },

        findClosestStops: function() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(app.showClosestStops);
            }
        },

		showClosestStopsRun: false,
		
        showClosestStops: function(position) {
			
			if(app.showClosestStopsRun && position == null)
				return;
			
			
            var nearbyStops = document.getElementsByClassName('nearby-stops')[0];

            // Calculate distances
			if(position != null) {
				app.showClosestStopsRun = true;
				
				app.busStops.forEach(function(busStop) {

					busStop.Distance = app._getDistance(position.coords.latitude, position.coords.longitude,
							busStop.Latitude, busStop.Longitude);
				});
			}

            var distanceOrderedStops = app.busStops.sort(function(a, b) { return a.Distance - b.Distance; });

            nearbyStops.innerHTML = '';
            var index = 0;


            // Render the closest 5 or any upto 500m away
            while (index <= 5 || distanceOrderedStops[index].Distance < 0.5) {

                nearbyStops.innerHTML +=
                    app._busStopTemplate(distanceOrderedStops[index]);

                index++;
            }

            // Next configure the search
            app.configureSearch();
        },

        forceRefresh: function(event) {
            app.updateBusTimes();
            
            // Reset the timer
            clearInterval(app.busUpdateInterval);
            app.busUpdateInterval = setInterval(function() {

                app.updateBusTimes();
            }, 1000 * 15); 
        },

        backToSearch: function(event) {

            document.body.className = 'transitioning';
            app.busStopLoockupScreen.className += ' active';
            app.arrivingSoonScreen.className = app.arrivingSoonScreen.className.replace(' active', '');

            setInterval(function(){
                document.body.className = '';
            }, 800)
            clearInterval(app.busUpdateInterval);
        },

        selectBusStop: function(event) {

            // Update screen state
            app.arrivingSoonScreen.className += ' active';
            app.busStopLoockupScreen.className = app.busStopLoockupScreen.className.replace(' active', '');

            var selectedId = null;
            var selectedElement = event.target;
            while (selectedId == null) {
                if (selectedElement.dataset != null && selectedElement.dataset.id != null) {
                    selectedId = selectedElement.dataset.id;
                    break;
                }
                selectedElement = selectedElement.parentElement;
            }

            app.currentBusStop = JSON.parse(localStorage.getItem(selectedId));

            // Render the stop name
            document.getElementsByClassName('selected-stop')[0].textContent = app.currentBusStop.Name.split(',')[0];

            // Clear any existing bus time data
            document.getElementsByClassName('upcoming-buses')[0].innerHTML = '';

            app.updateBusTimes();

            // Start an interval to update
            app.busUpdateInterval = setInterval(function() {

                app.updateBusTimes();
            }, 1000 * 15); // Update every 15 seconds
        },

        updateBusTimes: function() {

            app._showNotification('Loading bus times');

            fetch('/BusApi/GetNextBusTimes/' + app.currentBusStop.Id).then(function(response) {
                // Convert to JSON
                try {
                    return response.json();
                } catch (e) {
                    console.log('Error', e);
                    app._hideNotification();
                    return null;
                }
            }).then(function(data) {

                try {
                    var html = '';
                    data.forEach(function(bus) {

                        html += app._upcomingBusTemplate(bus);
                    });

                    document.getElementsByClassName('upcoming-buses')[0].innerHTML = html;
                } catch (e) {
                    console.log('Error', e);
                    app._hideNotification();
                } finally {
                    app._hideNotification();
                }
            });

        },

        registerNotifications: function (routeId) {

            if (!confirm('Subscribe to notifications?'))
                return;

            OneSignal.push(["sendTags", { routeId: routeId }]);
        },

        updateDynamicTimes: function() {

            var now = new Date();

            document.querySelectorAll('.dynamic-time').forEach(function(item) {
                var date = new Date(item.dataset.time);

                var secondsDifference = Math.floor((now - date) / 1000);
                var message = '';

                if (secondsDifference < 2) {
                    message = '1 second ago';
                } else if (secondsDifference < 60) {
                    message = secondsDifference+ ' seconds ago';

                } else if (secondsDifference < 60) {
                    message = secondsDifference+ ' seconds ago';

                } else if (secondsDifference < 120) {
                    message = (secondsDifference / 60).toFixed(0)+ ' minute ago';

                } else {

                    message = (secondsDifference / 60).toFixed(0)+ '  minutes ago';
                }

                item.innerHTML = message;
            });
        },

        _busStopTemplate: function(busStop, hidden) {

            var distance = '';

            if (busStop.Distance != null) {
                if (busStop.Distance < 1) {
                    var meters = busStop.Distance * 1000;
                    distance = '<p class="text-dark">' +meters.toFixed(0)+ 'm away</p>';
                } else {
                    distance = '<p class="text-dark">' +busStop.Distance.toFixed(2)+ 'km away</p>';
                }
            }

            return '<li data-id="' +busStop.Id+ '" data-name="' +busStop.Name+ '" class="' +(hidden ? 'hidden' : '')+ '">'+
                '<h3><span>' + busStop.Name + '</span></h3>'+
                    distance +
                '</li>';
        },

        _upcomingBusTemplate: function (bus) {
            
            return '<li>' +
                '<a href="#notify" onclick="app.registerNotifications(' + bus.id + ')" class="notify">' +
                '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" id="Capa_1" x="0px" y="0px" viewBox="0 0 60 60" style="enable-background:new 0 0 60 60;" xml:space="preserve" width="48px" height="48px">' +
                '<g>' +
                '<path d="M47.024,31.5v-8.994c0.043-6.857-4.568-11.405-8.53-13.216C37.359,8.771,36.189,8.371,35,8.074V7.5c0-2.757-2.243-5-5-5   s-5,2.243-5,5v0.661c-1.071,0.289-2.124,0.666-3.146,1.138C17.476,11.317,13.04,16.181,13,22.5v9   c0,6.388-2.256,11.869-6.705,16.291c-0.265,0.264-0.361,0.653-0.249,1.01s0.415,0.621,0.784,0.685l9.491,1.639   c1.768,0.305,3.396,0.555,4.945,0.761c1.745,3.42,5.172,5.615,8.916,5.615c3.745,0,7.173-2.196,8.917-5.618   c1.543-0.205,3.163-0.454,4.921-0.758l9.49-1.639c0.369-0.063,0.671-0.328,0.784-0.685c0.113-0.356,0.017-0.746-0.249-1.01   C49.387,43.16,47.024,37.679,47.024,31.5z M27,7.5c0-1.654,1.346-3,3-3s3,1.346,3,3v0.182c-1.993-0.286-4.015-0.274-6,0.047V7.5z    M30.183,55.5c-2.532,0-4.898-1.258-6.417-3.315c2.235,0.23,4.321,0.346,6.406,0.346c2.093,0,4.186-0.116,6.43-0.349   C35.082,54.241,32.715,55.5,30.183,55.5z M43.681,49.153c-1.919,0.331-3.678,0.6-5.34,0.812c-0.002,0-0.004,0-0.007,0   c-0.733,0.093-1.445,0.174-2.142,0.244c-0.006,0-0.011,0.001-0.017,0.001c-0.639,0.064-1.263,0.116-1.881,0.16   c-0.116,0.008-0.232,0.016-0.347,0.023c-0.535,0.035-1.064,0.063-1.59,0.083c-0.107,0.004-0.215,0.01-0.322,0.013   c-1.244,0.042-2.471,0.042-3.714,0.001c-0.111-0.004-0.223-0.01-0.334-0.014c-0.517-0.021-1.038-0.047-1.565-0.082   c-0.125-0.008-0.25-0.016-0.376-0.025c-0.599-0.043-1.205-0.093-1.824-0.155c-0.023-0.002-0.045-0.004-0.068-0.006   c-0.692-0.069-1.398-0.15-2.124-0.242c-0.003,0-0.006,0-0.009,0c-1.668-0.211-3.434-0.482-5.361-0.814L9,47.83   c3.983-4.554,6-10.038,6-16.33v-8.994c0.034-5.435,3.888-9.637,7.691-11.391c1.131-0.521,2.304-0.91,3.497-1.183   c0.01-0.002,0.021-0.001,0.031-0.003c2.465-0.554,5.087-0.579,7.58-0.068c0.013,0.003,0.026-0.003,0.039-0.001   c1.304,0.272,2.588,0.684,3.825,1.249c3.689,1.687,7.396,5.861,7.361,11.392v9c0,6.033,2.175,11.643,6.313,16.331L43.681,49.153z" fill="#87c301"/>' +
                '<path d="M36.417,13.838c-3.875-1.771-8.62-1.773-12.469,0.002c-2.195,1.012-5.918,3.973-5.948,8.654   c-0.003,0.552,0.441,1.002,0.994,1.006c0.002,0,0.004,0,0.006,0c0.549,0,0.997-0.443,1-0.994c0.023-3.677,3.019-6.035,4.785-6.85   c3.33-1.537,7.446-1.533,10.799,0c0.503,0.23,1.096,0.009,1.326-0.493C37.14,14.66,36.918,14.067,36.417,13.838z" fill="#87c301"/>' +
                '<path d="M4.802,22.793c-0.391-0.391-1.023-0.391-1.414,0C1.203,24.978,0,27.886,0,30.983c0,3.097,1.203,6.006,3.388,8.19   c0.195,0.195,0.451,0.293,0.707,0.293s0.512-0.098,0.707-0.293c0.391-0.391,0.391-1.023,0-1.414C2.995,35.952,2,33.546,2,30.983   c0-2.563,0.995-4.97,2.802-6.776C5.192,23.816,5.192,23.184,4.802,22.793z" fill="#87c301"/>' +
                '<path d="M8.305,37.19c0.256,0,0.512-0.098,0.707-0.293c0.391-0.391,0.391-1.023,0-1.414c-2.558-2.558-2.558-6.719,0-9.276   c0.391-0.391,0.391-1.023,0-1.414s-1.023-0.391-1.414,0c-3.337,3.337-3.337,8.768,0,12.104C7.793,37.093,8.049,37.19,8.305,37.19z" fill="#87c301"/>' +
                '<path d="M56.612,22.793c-0.391-0.391-1.023-0.391-1.414,0s-0.391,1.023,0,1.414C57.005,26.014,58,28.42,58,30.983   c0,2.563-0.995,4.969-2.802,6.776c-0.391,0.391-0.391,1.023,0,1.414c0.195,0.195,0.451,0.293,0.707,0.293s0.512-0.098,0.707-0.293   C58.797,36.989,60,34.08,60,30.983C60,27.886,58.797,24.978,56.612,22.793z" fill="#87c301"/>' +
                '<path d="M50.988,24.793c-0.391,0.391-0.391,1.023,0,1.414c2.558,2.558,2.558,6.719,0,9.276c-0.391,0.391-0.391,1.023,0,1.414   c0.195,0.195,0.451,0.293,0.707,0.293s0.512-0.098,0.707-0.293c3.337-3.337,3.337-8.768,0-12.104   C52.011,24.402,51.378,24.402,50.988,24.793z" fill= "#87c301" />' +
                '</g>' +
                '</svg>' +
                '</a>' +

                '<h3><em>' + bus.name + '</em> <span>' + bus.destination + '</span>' +
                '</h3>' +
                '<h2><em>' + (bus.arrivalTime || '30+') + '</em> <span>minutes</span></h2>' +
                '<small><i class="dynamic-time text-dark" data-time="' + new Date() + '">Updated 1 second ago</i></small>' +
                '</li>';
        },

        _getDistance: function(lat1, lon1, lat2, lon2) {
            function deg2rad(deg) {
                return deg * (Math.PI / 180)
            }

            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1); // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
                Math.sin(dLat / 2) * Math.sin(dLat / 2) +
                    Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
                    Math.sin(dLon / 2) * Math.sin(dLon / 2);
            var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        },

        _showNotification: function(text) {
            app.notification.innerHTML = text;
            app.notification.className = 'notification active';
        },

        _hideNotification: function() {

            app.notification.className = 'notification';
        }

    };

    window.addEventListener('load', function() {

        app.init();

        document.getElementById('force-refresh').addEventListener('click', app.forceRefresh);
        document.getElementById('back-to-search').addEventListener('click', app.backToSearch);
        document.getElementsByClassName('search')[0].addEventListener('keypress', app.searchStops);
    });
} catch (e) {

    alert(e);
}