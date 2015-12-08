/// <binding Clean='clean' />
'use strict';
/* jshint node: true */
var gulp = require("gulp"),
  rimraf = require("rimraf"),
  concat = require("gulp-concat"),
  cssmin = require("gulp-cssmin"),
  uglify = require("gulp-uglify"),
  shell = require("gulp-shell"),
  project = require("./project.json");

gulp.task('watch', shell.task(['dnx-watch web']));