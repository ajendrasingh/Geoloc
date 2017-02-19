<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false&libraries=places"></script>
    <script type="text/javascript">
        var source, destination;
        var directionsDisplay;
        //This is to add in github
        var retriction = { componentRestrictions: { 'country': 'in' } };

        var directionsService = new google.maps.DirectionsService();
        google.maps.event.addDomListener(window, 'load', function () {
            new google.maps.places.SearchBox(document.getElementById('txtSource'));
            new google.maps.places.SearchBox(document.getElementById('txtDestination'));
            directionsDisplay = new google.maps.DirectionsRenderer({ 'draggable': true });
        });

        function GetRoute() {
            var mumbai = new google.maps.LatLng(18.9750, 72.8258);
            var mapOptions = {
                zoom: 7,
                center: mumbai
            };
            map = new google.maps.Map(document.getElementById('dvMap'), mapOptions);
            directionsDisplay.setMap(map);
            directionsDisplay.setPanel(document.getElementById('dvPanel'));

            //*********DIRECTIONS AND ROUTE**********************//
            source = document.getElementById("txtSource").value;
            destination = document.getElementById("txtDestination").value;

            //source = "";
            alert(source);
            alert(destination);
            var request = {
                origin: source,
                destination: destination,
                travelMode: google.maps.TravelMode.DRIVING
            };
            directionsService.route(request, function (response, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(response);
                }
            });

            //*********DISTANCE AND DURATION**********************//
            //var service = new google.maps.DistanceMatrixService();
            //service.getDistanceMatrix({
            //    origins: [source],
            //    destinations: [destination],
            //    travelMode: google.maps.TravelMode.DRIVING,
            //    unitSystem: google.maps.UnitSystem.METRIC,
            //    avoidHighways: false,
            //    avoidTolls: false
            //}, function (response, status) {
            //    if (status == google.maps.DistanceMatrixStatus.OK && response.rows[0].elements[0].status != "ZERO_RESULTS") {
            //        var distance = response.rows[0].elements[0].distance.text;
            //        var duration = response.rows[0].elements[0].duration.text;
            //        var dvDistance = document.getElementById("dvDistance");
            //        dvDistance.innerHTML = "";
            //        dvDistance.innerHTML += "Distance: " + distance + "<br />";
            //        dvDistance.innerHTML += "Duration:" + duration;

            //    } else {
            //        alert("Unable to find the distance via road.");
            //    }
            //});
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtSource" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtDestination" runat="server"></asp:TextBox>

            <asp:Button ID="btnCal" runat="server" Text="Calculate" OnClick="btnCal_Click" />

            <input type="button" value="Get Route" onclick="GetRoute()" />

            <br />
            <br />
            <b>Time: </b>
            <asp:Label ID="lblDuration" runat="server"></asp:Label>
            <br />
            <b>Distance: </b>
            <asp:Label ID="lblDist" runat="server"></asp:Label>
        </div>
        <div id="dvDistance">
        </div>
        <div>
            <asp:GridView ID="grdMapping" runat="server"></asp:GridView>
        </div>
        <div id="dvMap" style="width: 500px; height: 500px">
        </div>
        <%--<div id="dvPanel" style="width: 500px; height: 500px">
        </div>--%>
    </form>
</body>
</html>
