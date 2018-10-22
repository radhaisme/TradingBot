class Loader {
	public static async load(): Promise<any> {
		var body = { ApiName: "Binance", Action: "GetTradePairsAsync", ModelType: "ProxyRequestModel" };
		var r = await fetch("proxy/execute", { method: "POST", body: JSON.stringify(body), headers: { "Content-Type": "application/json" } });
		console.log(await r.json());
	}
}

(async () => {
	await Loader.load();
})();