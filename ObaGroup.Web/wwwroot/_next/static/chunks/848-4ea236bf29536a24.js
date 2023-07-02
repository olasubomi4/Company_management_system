"use strict";(self.webpackChunk_N_E=self.webpackChunk_N_E||[]).push([[848],{67848:function(t,e,s){let r;s.d(e,{a:function(){return U}});var i=s(32161),n=s(30081),u=s(15761),l=s(33989),o=s(72379);class a extends l.l{constructor(t,e){super(),this.client=t,this.options=e,this.trackedProps=new Set,this.selectError=null,this.bindMethods(),this.setOptions(e)}bindMethods(){this.remove=this.remove.bind(this),this.refetch=this.refetch.bind(this)}onSubscribe(){1===this.listeners.length&&(this.currentQuery.addObserver(this),c(this.currentQuery,this.options)&&this.executeFetch(),this.updateTimers())}onUnsubscribe(){this.listeners.length||this.destroy()}shouldFetchOnReconnect(){return h(this.currentQuery,this.options,this.options.refetchOnReconnect)}shouldFetchOnWindowFocus(){return h(this.currentQuery,this.options,this.options.refetchOnWindowFocus)}destroy(){this.listeners=[],this.clearStaleTimeout(),this.clearRefetchInterval(),this.currentQuery.removeObserver(this)}setOptions(t,e){let s=this.options,r=this.currentQuery;if(this.options=this.client.defaultQueryOptions(t),(0,i.VS)(s,this.options)||this.client.getQueryCache().notify({type:"observerOptionsUpdated",query:this.currentQuery,observer:this}),void 0!==this.options.enabled&&"boolean"!=typeof this.options.enabled)throw Error("Expected enabled to be a boolean");this.options.queryKey||(this.options.queryKey=s.queryKey),this.updateQuery();let n=this.hasListeners();n&&d(this.currentQuery,r,this.options,s)&&this.executeFetch(),this.updateResult(e),n&&(this.currentQuery!==r||this.options.enabled!==s.enabled||this.options.staleTime!==s.staleTime)&&this.updateStaleTimeout();let u=this.computeRefetchInterval();n&&(this.currentQuery!==r||this.options.enabled!==s.enabled||u!==this.currentRefetchInterval)&&this.updateRefetchInterval(u)}getOptimisticResult(t){let e=this.client.getQueryCache().build(this.client,t);return this.createResult(e,t)}getCurrentResult(){return this.currentResult}trackResult(t){let e={};return Object.keys(t).forEach(s=>{Object.defineProperty(e,s,{configurable:!1,enumerable:!0,get:()=>(this.trackedProps.add(s),t[s])})}),e}getCurrentQuery(){return this.currentQuery}remove(){this.client.getQueryCache().remove(this.currentQuery)}refetch({refetchPage:t,...e}={}){return this.fetch({...e,meta:{refetchPage:t}})}fetchOptimistic(t){let e=this.client.defaultQueryOptions(t),s=this.client.getQueryCache().build(this.client,e);return s.isFetchingOptimistic=!0,s.fetch().then(()=>this.createResult(s,e))}fetch(t){var e;return this.executeFetch({...t,cancelRefetch:null==(e=t.cancelRefetch)||e}).then(()=>(this.updateResult(),this.currentResult))}executeFetch(t){this.updateQuery();let e=this.currentQuery.fetch(this.options,t);return null!=t&&t.throwOnError||(e=e.catch(i.ZT)),e}updateStaleTimeout(){if(this.clearStaleTimeout(),i.sk||this.currentResult.isStale||!(0,i.PN)(this.options.staleTime))return;let t=(0,i.Kp)(this.currentResult.dataUpdatedAt,this.options.staleTime);this.staleTimeoutId=setTimeout(()=>{this.currentResult.isStale||this.updateResult()},t+1)}computeRefetchInterval(){var t;return"function"==typeof this.options.refetchInterval?this.options.refetchInterval(this.currentResult.data,this.currentQuery):null!=(t=this.options.refetchInterval)&&t}updateRefetchInterval(t){this.clearRefetchInterval(),this.currentRefetchInterval=t,!i.sk&&!1!==this.options.enabled&&(0,i.PN)(this.currentRefetchInterval)&&0!==this.currentRefetchInterval&&(this.refetchIntervalId=setInterval(()=>{(this.options.refetchIntervalInBackground||u.j.isFocused())&&this.executeFetch()},this.currentRefetchInterval))}updateTimers(){this.updateStaleTimeout(),this.updateRefetchInterval(this.computeRefetchInterval())}clearStaleTimeout(){this.staleTimeoutId&&(clearTimeout(this.staleTimeoutId),this.staleTimeoutId=void 0)}clearRefetchInterval(){this.refetchIntervalId&&(clearInterval(this.refetchIntervalId),this.refetchIntervalId=void 0)}createResult(t,e){let s;let r=this.currentQuery,n=this.options,u=this.currentResult,l=this.currentResultState,a=this.currentResultOptions,h=t!==r,f=h?t.state:this.currentQueryInitialState,y=h?this.currentResult:this.previousQueryResult,{state:R}=t,{dataUpdatedAt:v,error:b,errorUpdatedAt:m,fetchStatus:Q,status:S}=R,I=!1,g=!1;if(e._optimisticResults){let s=this.hasListeners(),i=!s&&c(t,e),u=s&&d(t,r,e,n);(i||u)&&(Q=(0,o.Kw)(t.options.networkMode)?"fetching":"paused",v||(S="loading")),"isRestoring"===e._optimisticResults&&(Q="idle")}if(e.keepPreviousData&&!R.dataUpdatedAt&&null!=y&&y.isSuccess&&"error"!==S)s=y.data,v=y.dataUpdatedAt,S=y.status,I=!0;else if(e.select&&void 0!==R.data){if(u&&R.data===(null==l?void 0:l.data)&&e.select===this.selectFn)s=this.selectResult;else try{this.selectFn=e.select,s=e.select(R.data),s=(0,i.oE)(null==u?void 0:u.data,s,e),this.selectResult=s,this.selectError=null}catch(t){this.selectError=t}}else s=R.data;if(void 0!==e.placeholderData&&void 0===s&&"loading"===S){let t;if(null!=u&&u.isPlaceholderData&&e.placeholderData===(null==a?void 0:a.placeholderData))t=u.data;else if(t="function"==typeof e.placeholderData?e.placeholderData():e.placeholderData,e.select&&void 0!==t)try{t=e.select(t),this.selectError=null}catch(t){this.selectError=t}void 0!==t&&(S="success",s=(0,i.oE)(null==u?void 0:u.data,t,e),g=!0)}this.selectError&&(b=this.selectError,s=this.selectResult,m=Date.now(),S="error");let C="fetching"===Q,E="loading"===S,O="error"===S,T={status:S,fetchStatus:Q,isLoading:E,isSuccess:"success"===S,isError:O,isInitialLoading:E&&C,data:s,dataUpdatedAt:v,error:b,errorUpdatedAt:m,failureCount:R.fetchFailureCount,failureReason:R.fetchFailureReason,errorUpdateCount:R.errorUpdateCount,isFetched:R.dataUpdateCount>0||R.errorUpdateCount>0,isFetchedAfterMount:R.dataUpdateCount>f.dataUpdateCount||R.errorUpdateCount>f.errorUpdateCount,isFetching:C,isRefetching:C&&!E,isLoadingError:O&&0===R.dataUpdatedAt,isPaused:"paused"===Q,isPlaceholderData:g,isPreviousData:I,isRefetchError:O&&0!==R.dataUpdatedAt,isStale:p(t,e),refetch:this.refetch,remove:this.remove};return T}updateResult(t){let e=this.currentResult,s=this.createResult(this.currentQuery,this.options);if(this.currentResultState=this.currentQuery.state,this.currentResultOptions=this.options,(0,i.VS)(s,e))return;this.currentResult=s;let r={cache:!0};(null==t?void 0:t.listeners)!==!1&&(()=>{if(!e)return!0;let{notifyOnChangeProps:t}=this.options;if("all"===t||!t&&!this.trackedProps.size)return!0;let s=new Set(null!=t?t:this.trackedProps);return this.options.useErrorBoundary&&s.add("error"),Object.keys(this.currentResult).some(t=>{let r=this.currentResult[t]!==e[t];return r&&s.has(t)})})()&&(r.listeners=!0),this.notify({...r,...t})}updateQuery(){let t=this.client.getQueryCache().build(this.client,this.options);if(t===this.currentQuery)return;let e=this.currentQuery;this.currentQuery=t,this.currentQueryInitialState=t.state,this.previousQueryResult=this.currentResult,this.hasListeners()&&(null==e||e.removeObserver(this),t.addObserver(this))}onQueryUpdate(t){let e={};"success"===t.type?e.onSuccess=!t.manual:"error"!==t.type||(0,o.DV)(t.error)||(e.onError=!0),this.updateResult(e),this.hasListeners()&&this.updateTimers()}notify(t){n.V.batch(()=>{var e,s,r,i,n,u,l,o;t.onSuccess?(null==(e=(s=this.options).onSuccess)||e.call(s,this.currentResult.data),null==(r=(i=this.options).onSettled)||r.call(i,this.currentResult.data,null)):t.onError&&(null==(n=(u=this.options).onError)||n.call(u,this.currentResult.error),null==(l=(o=this.options).onSettled)||l.call(o,void 0,this.currentResult.error)),t.listeners&&this.listeners.forEach(t=>{t(this.currentResult)}),t.cache&&this.client.getQueryCache().notify({query:this.currentQuery,type:"observerResultsUpdated"})})}}function c(t,e){return!1!==e.enabled&&!t.state.dataUpdatedAt&&!("error"===t.state.status&&!1===e.retryOnMount)||t.state.dataUpdatedAt>0&&h(t,e,e.refetchOnMount)}function h(t,e,s){if(!1!==e.enabled){let r="function"==typeof s?s(t):s;return"always"===r||!1!==r&&p(t,e)}return!1}function d(t,e,s,r){return!1!==s.enabled&&(t!==e||!1===r.enabled)&&(!s.suspense||"error"!==t.state.status)&&p(t,s)}function p(t,e){return t.isStaleByTime(e.staleTime)}var f=s(67294),y=s(464);let R=f.createContext((r=!1,{clearReset:()=>{r=!1},reset:()=>{r=!0},isReset:()=>r})),v=()=>f.useContext(R);var b=s(85945);let m=f.createContext(!1),Q=()=>f.useContext(m);m.Provider;var S=s(24798);let I=(t,e)=>{(t.suspense||t.useErrorBoundary)&&!e.isReset()&&(t.retryOnMount=!1)},g=t=>{f.useEffect(()=>{t.clearReset()},[t])},C=({result:t,errorResetBoundary:e,useErrorBoundary:s,query:r})=>t.isError&&!e.isReset()&&!t.isFetching&&(0,S.L)(s,[t.error,r]),E=t=>{t.suspense&&"number"!=typeof t.staleTime&&(t.staleTime=1e3)},O=(t,e)=>t.isLoading&&t.isFetching&&!e,T=(t,e,s)=>(null==t?void 0:t.suspense)&&O(e,s),F=(t,e,s)=>e.fetchOptimistic(t).then(({data:e})=>{null==t.onSuccess||t.onSuccess(e),null==t.onSettled||t.onSettled(e,null)}).catch(e=>{s.clearReset(),null==t.onError||t.onError(e),null==t.onSettled||t.onSettled(void 0,e)});function U(t,e,s){let r=(0,i._v)(t,e,s);return function(t,e){let s=(0,b.NL)({context:t.context}),r=Q(),i=v(),u=s.defaultQueryOptions(t);u._optimisticResults=r?"isRestoring":"optimistic",u.onError&&(u.onError=n.V.batchCalls(u.onError)),u.onSuccess&&(u.onSuccess=n.V.batchCalls(u.onSuccess)),u.onSettled&&(u.onSettled=n.V.batchCalls(u.onSettled)),E(u),I(u,i),g(i);let[l]=f.useState(()=>new e(s,u)),o=l.getOptimisticResult(u);if((0,y.$)(f.useCallback(t=>r?()=>void 0:l.subscribe(n.V.batchCalls(t)),[l,r]),()=>l.getCurrentResult(),()=>l.getCurrentResult()),f.useEffect(()=>{l.setOptions(u,{listeners:!1})},[u,l]),T(u,o,r))throw F(u,l,i);if(C({result:o,errorResetBoundary:i,useErrorBoundary:u.useErrorBoundary,query:l.getCurrentQuery()}))throw o.error;return u.notifyOnChangeProps?o:l.trackResult(o)}(r,a)}}}]);