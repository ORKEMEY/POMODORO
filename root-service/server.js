const express = require('express');
const app = express();
const server = require('http').createServer(app);
const port = 8080;

app.listen(port, (err) => {
    if (err) {
        return console.log('something bad happened', err)
    }
    console.log(`server is listening on ${port}`)
});

app.use('/api/index', express.static(`${__dirname}/public`));

