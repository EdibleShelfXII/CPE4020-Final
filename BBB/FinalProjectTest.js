
/* Required modules */
const http = require('http');
const url = require('url');
const path = require('path');
var b = require('bonescript');


/* Networking Variables */
const HOST = '10.0.0.185'; // BBB local IP
const PORT = 2000; // BBB server port

// init temperature data variables
var temp = "-9";
var tempLuis1 = "-9";
var tempLuis2 = "-9";
var tempArafat1 = "-9";
var tempArafat2 = "-9";
var tempIsaac1 = "-9";
var tempIsaac2 = "-9";


console.log("server started");


http.createServer(function (req, res) {
	//start http server
    
    if (req.url === path.normalize('/')) {

    res.end("0");
	
	
	// API calls and responses
    } else if (req.url === path.normalize('/api/sensor/all')) {

    
    console.log("Displaying to client: user:luis1:" + tempLuis1 + ",user:luis2:" + tempLuis2 + ",user:arafat1:" + tempArafat1 + ",user:arafat2:" + tempArafat2 + ",user:isaac1:" + tempIsaac1 + ",user:isaac2:" + tempIsaac2);
    res.write("user:luis1:" + tempLuis1 + ",user:luis2:" + tempLuis2 + ",user:arafat1:" + tempArafat1 + ",user:arafat2:" + tempArafat2 + ",user:isaac1:" + tempIsaac1 + ",user:isaac2:" + tempIsaac2);
    res.end();
    
    } else if (req.url === path.normalize('/api/sensor/luis')) {

    
    console.log("Displaying to client: user:luis1:" + tempLuis1 + ",user:luis2:" + tempLuis2);
    res.write("user:luis1:" + tempLuis1 + ",user:luis2:" + tempLuis2);
    res.end();
    
    } else if (req.url === path.normalize('/api/sensor/arafat')) {

    
    console.log("Displaying to client: user:arafat1:" + tempArafat1 + ",user:arafat2:" + tempArafat2);
    res.write("user:arafat1:" + tempArafat1 + ",user:arafat2:" + tempArafat2);
    res.end();
    
    } else if (req.url === path.normalize('/api/sensor/isaac')) {

    
    console.log("Displaying to client: user:isaac1:" + tempIsaac1 + ",user:isaac2:" + tempIsaac2);
    res.write("user:isaac1:" + tempIsaac1 + ",user:isaac2:" + tempIsaac2);
    res.end();
    
    } else {

    
    res.writeHead(200, {'Content-Type': 'text/html'});
    // Use the url module to turn the querystring into an object:
    var q = url.parse(req.url, true).query;
    // Return data and user from the query object:
    var txt = q.data + " " + q.user;
    console.log(txt);
	// Sort and log incoming data
    if ((q.data > 0) && (q.user == "luis1")) { tempLuis1 = q.data; console.log("user1 verified") }
    else if ((q.data > 0) && (q.user == "luis2")) { tempLuis2 = q.data; console.log("user2 verified") }
    else if ((q.data > 0) && (q.user == "arafat1")) { tempArafat1 = q.data; console.log("user3 verified") }
    else if ((q.data > 0) && (q.user == "arafat2")) { tempArafat2 = q.data; console.log("user4 verified") }
    else if ((q.data > 0) && (q.user == "isaac1")) { tempIsaac1 = q.data; console.log("user5 verified") }
    else if ((q.data > 0) && (q.user == "isaac2")) { tempIsaac2 = q.data; console.log("user6 verified") }
    else { console.log("not verified"); }
    res.end("Page not found");
    }
}).listen(PORT,HOST);
