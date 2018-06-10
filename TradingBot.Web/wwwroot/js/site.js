fetch("/home/getpairs", { method: "GET" })
	.then((response) => {
		return response.json();
	})
	.then((json) => {
		console.log(json);
	});