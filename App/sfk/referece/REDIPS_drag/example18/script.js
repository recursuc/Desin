/*jslint white: true, browser: true, undef: true, nomen: true, eqeqeq: true, plusplus: false, bitwise: true, regexp: true, strict: true, newcap: true, immed: true, maxerr: 14 */
/*global window: false, REDIPS: true */

/* enable strict mode */
"use strict";

/* Simple element animation */

// create redips container
var redips = {};


// redips initialization
redips.init = function () {
	// set reference to the REDIPS.drag library
	var rd = REDIPS.drag;
	// script configuration
	redips.clone = false;		// set false for "clone" checkbox
	redips.overwrite = false;	// set false to "overwrite" checkbox
	redips.button = document.getElementById('btn_move');	// set button reference (needed in redips.button_enable() method)
	// initialization
	rd.init();
	// animation pause (lower values mean the animation plays faster)
	rd.animation_pause = 40;
	// animation step (minimum is 1)
	rd.animation_step = 2;
	// event handler invoked after DIV element is cloned - called from REDIPS.drag.move_object()
	rd.myhandler_cloned = function (div_cloned) {
		div_cloned.style.borderColor = 'LightBlue';
	};
};


/**
 * Function moves element to the random position. Generated position must be different then current position.
 */
redips.move = function () {
	var id = 'd1',	// id of drag element
		rowIndex,	// row index (random number from 0 to 6)
		cellIndex,	// cell index (random number from 0 to 6)
		pos;		// current position as array (returned from get_position method)
	// set current position for DIV element with defined id
	pos = REDIPS.drag.get_position(id);
	// generate random position (must be different then current position)
	do {
		rowIndex = Math.floor(Math.random() * 7);	// from 0 to 6
		cellIndex = Math.floor(Math.random() * 7);	// from 0 to 6
	} while (pos[1] === rowIndex && pos[2] === cellIndex);
	// disable "Move" button
	redips.button_enable(false);
	// move object to the random position
	REDIPS.drag.move_object({
		id: id,								// id of object to move
		clone: redips.clone,				// clone option (if set to true then DIV element will be cloned)
		overwrite: redips.overwrite,		// overwrite target cell (if set to true, then content in target cell will be overwritten)
		target: [0, rowIndex, cellIndex],	// target position
		callback: redips.button_enable		// function to call after animation is over
		//callback: redips.move				// try to comment upper line and uncomment this line (refresh page and click on "Move" button)
	});
};


/**
 * Function enables/disables button.
 * @param {Boolean} Flag enable or disable buttons.
 */
redips.button_enable = function (flag) {
	// input parameter is optional (default value is true)
	if (flag === undefined) {
		flag = true;
	}
	// enable/disable button (reference is set in redips.init)
	redips.button.disabled = !flag;
};


// toggle clone option
redips.toggle_clone = function (chk) {
	redips.clone = chk.checked;
};


// toggle overwrite option
redips.toggle_overwrite = function (chk) {
	redips.overwrite = chk.checked;
};


// add onload event listener
if (window.addEventListener) {
	window.addEventListener('load', redips.init, false);
}
else if (window.attachEvent) {
	window.attachEvent('onload', redips.init);
}