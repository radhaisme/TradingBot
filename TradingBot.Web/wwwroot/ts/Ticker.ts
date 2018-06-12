class Ticker {
	private readonly _pair: string;
	private readonly _interval: number;

	constructor(pair: string, interval: number = 3000) {
		this._pair = pair;
		this._interval = interval;

		//fetch("/home/getpairs", { method: "GET" })
		//	.then((response) => {
		//		return response.json();
		//	})
		//	.then((json) => {
		//		console.log(json);
		//	});
		setInterval(() => {
			fetch("/home/getpairdata?pair=" + this._pair, { method: "GET" })
				.then((response) => {
					return response.json();
				})
				.then((json) => {
					console.log(json);
				});
		}, this._interval);
	}
}

let ticker = new Ticker("ltc_btc");