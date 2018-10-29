import TradePair from "./models/TradePair";
import IProxyRequest from "./IProxyRequest";
import ITradePairsResponse from "./models/ITradePairsResponse";

export default class Connector {
    // public async Call<TResponse>(request: IProxyRequest): Promise<TResponse> {

    //     let response: Response = await fetch("api/proxy/execute", {
    //         method: "POST",
    //         body: JSON.stringify(request),
    //         headers: { "Accept": "application/json", "Content-Type": "application/json" }
    //     });
    //     let model = <TResponse>JSON.parse(await response.json());

    //     return model;
    // }

    public async GetTradePairsAsync(request: IProxyRequest): Promise<ITradePairsResponse> {
        var response = await fetch(`api/${request.apiName}/GetTradePairsAsync`, { method: "GET" });

        return response.json();
    }
}