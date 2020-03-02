const express = require("express");
const bodyParser = require("body-parser");
const cors = require("cors");

const app = express();

app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());
app.use(cors());

process.on("uncaughtException", function(err) {
  console.log(err.stack);
});

try {
  console.log("Starting web server...");

  const port = process.env.PORT || 4000;
  app.listen(port, () => console.log(`Server started on: ${port}`));
} catch (error) {
  console.error(error.stack);
}

module.exports = app;
