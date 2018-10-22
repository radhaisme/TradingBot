var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
class Loader {
    static load() {
        return __awaiter(this, void 0, void 0, function* () {
            var body = { ApiName: "Binance", Action: "GetTradePairsAsync", ModelType: "ProxyRequestModel" };
            var r = yield fetch("proxy/execute", { method: "POST", body: JSON.stringify(body), headers: { "Content-Type": "application/json" } });
            console.log(yield r.json());
        });
    }
}
(() => __awaiter(this, void 0, void 0, function* () {
    yield Loader.load();
}))();
//# sourceMappingURL=site.js.map