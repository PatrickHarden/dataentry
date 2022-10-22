console.log('***** Postbuild script - START *****');
var fs = require('fs');
var fileName = './build/manifest.json';
var file = require(fileName);

for (var attributename in file) {
  if (attributename.startsWith('static/js/')) {
    console.log('writing vendor.js: ' + file[attributename]);
    file['vendor.js'] = file[attributename];
    break;
  }
}

fs.writeFile(fileName, JSON.stringify(file), function(err) {
  if (err) return console.log(err);
  console.log('***** Postbuild script - END *****');
});