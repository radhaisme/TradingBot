var Ticker = /** @class */ (function () {
    function Ticker(pair, interval) {
        if (interval === void 0) { interval = 3000; }
        var _this = this;
        this._pair = pair;
        this._interval = interval;
        //fetch("/home/getpairs", { method: "GET" })
        //	.then((response) => {
        //		return response.json();
        //	})
        //	.then((json) => {
        //		console.log(json);
        //	});
        setInterval(function () {
            fetch("/home/getpairdata?pair=" + _this._pair, { method: "GET" })
                .then(function (response) {
                return response.json();
            })
                .then(function (json) {
                console.log(json);
            });
        }, this._interval);
    }
    return Ticker;
}());
var ticker = new Ticker("ltc_btc");
//# sourceMappingURL=site.js.map