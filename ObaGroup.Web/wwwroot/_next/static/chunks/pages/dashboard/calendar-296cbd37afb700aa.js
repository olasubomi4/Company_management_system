(self.webpackChunk_N_E=self.webpackChunk_N_E||[]).push([[259],{36041:function(e,t,a){(window.__NEXT_P=window.__NEXT_P||[]).push(["/dashboard/calendar",function(){return a(43372)}])},85825:function(e,t,a){"use strict";var n=a(85893),i=a(17064);let r=e=>{let{className:t,onClick:a,children:r,type:d,variant:l="contained",loading:s}=e;return(0,n.jsx)(i.E.button,{whileTap:{y:1.5},onClick:a,type:d,className:"rounded-md capitalize p-2  min-w-[100px] hover:bg-[#009ABC] flex justify-center items ".concat("outlined"===l?"bg-transparent text-textColor hover:bg-slate-100 border-2":"text-white bg-highlight"," ").concat(t),children:s?(0,n.jsx)("div",{className:"loader-async"}):r})};t.Z=r},99537:function(e,t,a){"use strict";var n=a(85893);let i=e=>{let{className:t,children:a}=e;return(0,n.jsx)("div",{className:"bg-[#ffffff] rounded-md shadow-sm w-full p-4 ".concat(t),children:a})};t.Z=i},43372:function(e,t,a){"use strict";a.r(t),a.d(t,{default:function(){return j}});var n=a(85893),i=a(85825),r=a(96827),d=a(28624);a(36775);var l=a(29387),s=a.n(l),o=a(27484),u=a.n(o),c=a(67294),m=a(48228),v=a(53641);u().extend(s());let f=(0,r.VL)(u()),h=(0,d.Z)(r.f),p={container:{width:"80wh",height:"60vh",margin:"2em"}};function D(e){let{event:t=[]}=e,a=new FormData,i=(0,m.D)(e=>(a.append("StartDateTime",u()(e.startDateTime.toISOString()).format("YYYY-MM-DDTHH:mm")),a.append("EndDateTime",u()(e.endDateTime.toISOString()).format("YYYY-MM-DDTHH:mm")),a.append("EventId",e.id),v.p.request({url:"/Dashboard/Calendar/PatchAnEvent",method:"POST",data:a,headers:{"Content-Type":"multipart/form-data"}}))),[d,l]=(0,c.useState)([]);(0,c.useEffect)(()=>{l(t)},[t]);let s=(0,c.useCallback)(e=>{let{event:t,start:a,end:n,isAllDay:r=!1}=e,{allDay:d}=t;!d&&r&&(t.allDay=!0),l(e=>{var r;let l=null!==(r=e.find(e=>e.id===t.id))&&void 0!==r?r:{},s=e.filter(e=>e.id!==t.id);return i.mutate({startDateTime:a,endDateTime:n,id:l.id}),[...s,{...l,start:a,end:n,allDay:d}]})},[l]),o=(0,c.useCallback)(e=>{let{event:t,start:a,end:n}=e;l(e=>{var i;let r=null!==(i=e.find(e=>e.id===t.id))&&void 0!==i?i:{},d=e.filter(e=>e.id!==t.id);return[...d,{...r,start:a,end:n}]})},[l]);return(0,n.jsx)("div",{style:p.container,children:(0,n.jsx)(h,{selectable:!0,localizer:f,events:d,defaultView:r.kO.MONTH,views:[r.kO.DAY,r.kO.WEEK,r.kO.MONTH],steps:60,defaultDate:new Date,onEventDrop:s,onEventResize:o})})}var x=a(99537),T=a(67848),w=a(41664),E=a.n(w),b=a(11163);let g=()=>{var e;let t=(0,b.useRouter)(),{data:a}=(0,T.a)(["events"],()=>v.p.request({method:"get",url:"Dashboard/Calendar/ListEvents?EventMaxTimeRange=2024-04-30T11:50&EventMinTimeRange=2023-04-20T11:50"}),{onSuccess:e=>{var a;(null===(a=e.data)||void 0===a?void 0:a.items)||t.push(e.data)}}),r=null===(e=null==a?void 0:a.data.items)||void 0===e?void 0:e.map((e,t)=>({id:t,title:e.summary,start:new Date(e.start.dateTime),end:new Date(e.end.dateTime)}));return(0,n.jsxs)("div",{children:[(0,n.jsx)(x.Z,{className:"my-4",children:(0,n.jsxs)("div",{className:"flex justify-between my-4",children:[(0,n.jsx)("div",{className:"flex gap-4"}),(0,n.jsx)(E(),{href:"/dashboard/calendar/create",children:(0,n.jsx)(i.Z,{children:"Create Event"})})]})}),(0,n.jsx)(D,{event:r})]})};var j=g}},function(e){e.O(0,[770,115,774,888,179],function(){return e(e.s=36041)}),_N_E=e.O()}]);