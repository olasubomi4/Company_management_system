(self.webpackChunk_N_E=self.webpackChunk_N_E||[]).push([[259],{36041:function(e,t,n){(window.__NEXT_P=window.__NEXT_P||[]).push(["/dashboard/calendar",function(){return n(43372)}])},85825:function(e,t,n){"use strict";var a=n(85893),l=n(17064);let s=e=>{let{className:t,onClick:n,children:s,type:i,variant:r="contained",loading:d}=e;return(0,a.jsx)(l.E.button,{whileTap:{y:1.5},onClick:n,type:i,className:"rounded-md capitalize p-2  min-w-[100px] hover:bg-[#009ABC] flex justify-center items ".concat("outlined"===r?"bg-transparent text-textColor hover:bg-slate-100 border-2":"text-white bg-highlight"," ").concat(t),children:d?(0,a.jsx)("div",{className:"loader-async"}):s})};t.Z=s},99537:function(e,t,n){"use strict";var a=n(85893);let l=e=>{let{className:t,children:n}=e;return(0,a.jsx)("div",{className:"bg-[#ffffff] rounded-md shadow-sm w-full p-4 ".concat(t),children:n})};t.Z=l},90197:function(e,t,n){"use strict";var a=n(85893);n(67294);var l=n(9198),s=n.n(l);let i=e=>{let{value:t,onChange:n,name:l,inline:i,placeholderText:r,showTimeSelect:d,dateFormat:c}=e;return(0,a.jsx)(s(),{selected:t,onChange:n,className:"border-[1px] border-slate-300 rounded-lg w-full p-2 outline-highlight hover:border-slate-800 bg-transparent z-100",placeholderText:r||"Click to select date",name:l,inline:i,showTimeSelect:d,showIcon:!0,dateFormat:c})};t.Z=i},43372:function(e,t,n){"use strict";n.r(t),n.d(t,{default:function(){return T}});var a=n(85893),l=n(85825),s=n(96827),i=n(28624);n(36775);var r=n(29387),d=n.n(r),c=n(27484),o=n.n(c),u=n(67294);o().extend(d());let h=(0,s.VL)(o()),v=(0,i.Z)(s.f),f={container:{width:"80wh",height:"60vh",margin:"2em"}};function m(e){let{event:t=[]}=e,[n,l]=(0,u.useState)([]);(0,u.useEffect)(()=>{l(t)},[t]);let i=(0,u.useCallback)(e=>{let{event:t,start:n,end:a,isAllDay:s=!1}=e,{allDay:i}=t;!i&&s&&(t.allDay=!0),l(e=>{var l;let s=null!==(l=e.find(e=>e.id===t.id))&&void 0!==l?l:{},r=e.filter(e=>e.id!==t.id);return[...r,{...s,start:n,end:a,allDay:i}]})},[l]),r=(0,u.useCallback)(e=>{let{event:t,start:n,end:a}=e;l(e=>{var l;let s=null!==(l=e.find(e=>e.id===t.id))&&void 0!==l?l:{},i=e.filter(e=>e.id!==t.id);return[...i,{...s,start:n,end:a}]})},[l]);return(0,a.jsx)("div",{style:f.container,children:(0,a.jsx)(v,{selectable:!0,localizer:h,events:n,defaultView:s.kO.MONTH,views:[s.kO.DAY,s.kO.WEEK,s.kO.MONTH],steps:60,defaultDate:new Date,onEventDrop:i,onEventResize:r})})}var x=n(99537),p=n(90197),j=n(53641),w=n(67848),b=n(41664),g=n.n(b),E=n(11163);let N=()=>{var e;let t=(0,E.useRouter)(),{data:n}=(0,w.a)(["events"],()=>j.p.request({method:"get",url:"Dashboard/Calendar/ListEvents?EventMaxTimeRange=2024-04-30T11:50&EventMinTimeRange=2023-04-20T11:50"}),{onSuccess:e=>{var n;(null===(n=e.data)||void 0===n?void 0:n.items)||t.push(e.data)}}),s=null===(e=null==n?void 0:n.data.items)||void 0===e?void 0:e.map((e,t)=>({id:t,title:e.summary,start:new Date(e.start.dateTime),end:new Date(e.end.dateTime)}));return(0,a.jsxs)("div",{children:[(0,a.jsxs)(x.Z,{className:"my-4",children:[(0,a.jsx)("h3",{children:"Filter"}),(0,a.jsxs)("div",{className:"flex justify-between my-4",children:[(0,a.jsxs)("div",{className:"flex gap-4",children:[(0,a.jsxs)("div",{className:"",children:[(0,a.jsx)("p",{children:"Start Date"}),(0,a.jsx)(p.Z,{})]}),(0,a.jsxs)("div",{className:"",children:[(0,a.jsx)("p",{children:"End Date"}),(0,a.jsx)(p.Z,{})]})]}),(0,a.jsx)(g(),{href:"/dashboard/calendar/create",children:(0,a.jsx)(l.Z,{children:"Create Event"})})]})]}),(0,a.jsx)(m,{event:s})]})};var T=N}},function(e){e.O(0,[770,198,115,774,888,179],function(){return e(e.s=36041)}),_N_E=e.O()}]);