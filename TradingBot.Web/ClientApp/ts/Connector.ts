export default class Connector {
    public async Call<TResponse>(request: IProxyRequest): Promise<TResponse> {

        let response: Response = await fetch("api/proxy/execute", {
            method: "POST",
            body: JSON.stringify(request),
            headers: { "Accept": "application/json", "Content-Type": "application/json" }
        });
        let model = <TResponse>JSON.parse(await response.json());

        return model;
    }
}