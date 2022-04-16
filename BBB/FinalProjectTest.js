/**** Edit this file to integrate your Temperature Sensing code **

/* Required modules */
const http = require('http');
const url = require('url');
const path = require('path');
var b = require('bonescript');


/* Networking Variables */
const HOST = '10.0.0.185';
const PORT = 2000;

var temp = "-9";
var tempLuis1 = "-9";

console.log("server started");


http.createServer(function (req, res) {
    
    if (req.url === path.normalize('/')) {

    res.end("0");

    } else if (req.url === path.normalize('/api/sensor')) {

    /**** Return the API call result. Change X0 to your lab group number. Change units:X to either C or F ****/
    console.log("Displaying to client: " + tempLuis1);
    res.write("" + tempLuis1);
    res.end();
    
    } else {

    
    res.writeHead(200, {'Content-Type': 'text/html'});
    /*Use the url module to turn the querystring into an object:*/
    var q = url.parse(req.url, true).query;
    /*Return the year and month from the query object:*/
    var txt = q.data + " " + q.user;
    console.log(txt);
    if ((q.data > 0) && (q.user == "luis1")) { tempLuis1 = q.data; console.log("user verified") };
    res.end(txt);
    }
}).listen(PORT,HOST);
