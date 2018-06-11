fetch("/home/getpairs", { method: "GET" })
	.then((response) => {
		return response.json();
	})
	.then((json) => {
		console.log(json);
	});
setInterval(() => {
	fetch("/home/getpairdata" + "?pair=ltc_btc", { method: "GET" })
		.then((response) => {
			return response.json();
		})
		.then((json) => {
			console.log(json);
		});
}, 3000);