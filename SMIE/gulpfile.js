﻿/// <binding AfterBuild='build' />
'use strict';

var gulp = require('gulp');
var less = require('gulp-less');
var lessGlobPlugin = require('less-plugin-glob');
var postcss = require('gulp-postcss');
var autoprefixer = require('autoprefixer');
var minifyCss = require('gulp-csso');
var minifyJs = require('gulp-uglify');
var rename = require('gulp-rename');
var plumber = require('gulp-plumber');

// Styles -----------------------------------------------------------------

var stylesStorage = "./wwwroot/dist/styles/pages/";
var pageStyles = "./wwwroot/css/pages/**/*.css";

gulp.task("css", function () {
    return gulp.src([pageStyles])
        .pipe(plumber({ errorHandler: handleError }))
        .pipe(less({ plugins: [lessGlobPlugin] }))
        .pipe(postcss([autoprefixer({ browsers: ['last 8 versions'] })]))
        .pipe(gulp.dest(stylesStorage))
        .pipe(minifyCss())
        .pipe(rename({ suffix: ".min" }))
        .pipe(gulp.dest(stylesStorage));
});

// Scripts ----------------------------------------------------------------

var scriptsStorage = "./wwwroot/dist/js/pages/";
var pageScripts = "./wwwroot/js/pages/**/*.js";

gulp.task("js", function () {
    return gulp.src(pageScripts)
        .pipe(plumber({ errorHandler: handleError }))
        .pipe(gulp.dest(scriptsStorage));
});

// Grouped build task -----------------------------------------------------

gulp.task("build", [
    "css",
    "js"
]);

// Watching ---------------------------------------------------------------

gulp.task("watch", function () {
    gulp.watch([
        pageStyles,
        pageScripts
    ], ["build"]);
});

// Helper stuff -----------------------------------------------------------

function handleError(err) {
    console.log(err.toString());
    this.emit("end");
}
