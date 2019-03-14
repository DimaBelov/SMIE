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

gulp.task("css-npm", function () {
    return gulp.src([
            "./node_modules/video.js/dist/video-js.min.css",
            "./node_modules/bootstrap-material-design/dist/css/bootstrap-material-design.min.css"
        ])
        .pipe(plumber({ errorHandler: handleError }))
        .pipe(gulp.dest("./wwwroot/dist/styles/"));
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

gulp.task("js-npm", function () {
    return gulp.src([
        "./node_modules/video.js/dist/video.min.js",
        "./node_modules/bootstrap-material-design/dist/js/bootstrap-material-design.min.js"
    ])
    .pipe(plumber({ errorHandler: handleError }))
    .pipe(gulp.dest("./wwwroot/dist/scripts/"));
});

// Grouped build task -----------------------------------------------------

gulp.task("build", [
    "css-pages",
    "css-components",
    "css-npm",
    "js-pages",
    "js-components",
    "js-npm"
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
