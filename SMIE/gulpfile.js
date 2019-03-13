/// <binding AfterBuild='build' />
'use strict';

var gulp = require('gulp');
var minifyCss = require('gulp-csso');
var minifyJs = require('gulp-uglify');
var rename = require('gulp-rename');
var plumber = require('gulp-plumber');

// Styles -----------------------------------------------------------------

var stylesStorage = "./wwwroot/dist/styles/";
var pageStyles = "./wwwroot/css/pages/**/*.css";
var componentsStyles = "./wwwroot/css/components/**/*.css";

gulp.task("css-pages", function () {
    return gulp.src([pageStyles])
        .pipe(plumber({ errorHandler: handleError }))
        .pipe(minifyCss())
        .pipe(rename({ suffix: ".min" }))
        .pipe(gulp.dest(stylesStorage + 'pages/'));
});

gulp.task("css-components", function () {
    return gulp.src([componentsStyles])
        .pipe(plumber({ errorHandler: handleError }))
        .pipe(minifyCss())
        .pipe(rename({ suffix: ".min" }))
        .pipe(gulp.dest(stylesStorage + 'components/'));
});

// Scripts ----------------------------------------------------------------

var scriptsStorage = "./wwwroot/dist/scripts/";
var pageScripts = "./wwwroot/js/pages/**/*.js";
var componentsScripts = "./wwwroot/js/components/**/*.js";

gulp.task("js-pages", function () {
    return gulp.src(pageScripts)
        .pipe(plumber({ errorHandler: handleError }))
        .pipe(minifyJs())
        .pipe(rename({ suffix: ".min" }))
        .pipe(gulp.dest(scriptsStorage + 'pages/'));
});

gulp.task("js-components", function () {
    return gulp.src(componentsScripts)
        .pipe(plumber({ errorHandler: handleError }))
        .pipe(minifyJs())
        .pipe(rename({ suffix: ".min" }))
        .pipe(gulp.dest(scriptsStorage + 'components/'));
});

// Grouped build task -----------------------------------------------------

gulp.task("build", [
    "css-pages",
    "css-components",
    "js-pages",
    "js-components"
]);

// Watching ---------------------------------------------------------------

gulp.task("watch", function () {
    gulp.watch([
        pageStyles,
        componentsStyles,
        pageScripts,
        componentsScripts
    ], ["build"]);
});

// Helper stuff -----------------------------------------------------------

function handleError(err) {
    console.log(err.toString());
    this.emit("end");
}
