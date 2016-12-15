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

        showClosestStops: function(position) {
            var nearbyStops = document.getElementsByClassName('nearby-stops')[0];

            // Calculate distances
            app.busStops.forEach(function(busStop) {

                busStop.Distance = app._getDistance(position.coords.latitude, position.coords.longitude,
                    busStop.Latitude, busStop.Longitude);
            });

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
                    distance = '<p>' +meters.toFixed(0)+ 'm away</p>';
                } else {
                distance = '<p>' +busStop.Distance.toFixed(2)+ 'km away</p>';
                }
            }

            return '<li data-id="' +busStop.Id+ '" data-name="' +busStop.Name+ '" class="' +(hidden ? 'hidden' : '')+ '">'+
                    '<h3>' +busStop.Name+ '</h3>'+
                    distance +
                '</li>';
        },

        _upcomingBusTemplate: function(bus) {
            return '<li>' +
                    '<h3><em>' +bus.name+ '</em> ' +bus.destination+ '</h3>' +
                    '<h2><em>' +(bus.arrivalTime || '30+')+ '</em> minutes</h2>' +
                    '<small><i class="dynamic-time" data-time="' + new Date()+ '">Updated 1 second ago</i></small>'+
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