(self.webpackChunk_N_E=self.webpackChunk_N_E||[]).push([[187],{27484:function(t){var e,n,r,i,a,s,o,u,l,c,d,h,f,m,p,g,$,y,D,v,w;t.exports=(e="millisecond",n="second",r="minute",i="hour",a="week",s="month",o="quarter",u="year",l="date",c="Invalid Date",d=/^(\d{4})[-/]?(\d{1,2})?[-/]?(\d{0,2})[Tt\s]*(\d{1,2})?:?(\d{1,2})?:?(\d{1,2})?[.:]?(\d+)?$/,h=/\[([^\]]+)]|Y{1,4}|M{1,4}|D{1,2}|d{1,4}|H{1,2}|h{1,2}|a|A|m{1,2}|s{1,2}|Z{1,2}|SSS/g,f=function(t,e,n){var r=String(t);return!r||r.length>=e?t:""+Array(e+1-r.length).join(n)+t},(p={})[m="en"]={name:"en",weekdays:"Sunday_Monday_Tuesday_Wednesday_Thursday_Friday_Saturday".split("_"),months:"January_February_March_April_May_June_July_August_September_October_November_December".split("_"),ordinal:function(t){var e=["th","st","nd","rd"],n=t%100;return"["+t+(e[(n-20)%10]||e[n]||"th")+"]"}},g=function(t){return t instanceof v},$=function t(e,n,r){var i;if(!e)return m;if("string"==typeof e){var a=e.toLowerCase();p[a]&&(i=a),n&&(p[a]=n,i=a);var s=e.split("-");if(!i&&s.length>1)return t(s[0])}else{var o=e.name;p[o]=e,i=o}return!r&&i&&(m=i),i||!r&&m},y=function(t,e){if(g(t))return t.clone();var n="object"==typeof e?e:{};return n.date=t,n.args=arguments,new v(n)},(D={s:f,z:function(t){var e=-t.utcOffset(),n=Math.abs(e);return(e<=0?"+":"-")+f(Math.floor(n/60),2,"0")+":"+f(n%60,2,"0")},m:function t(e,n){if(e.date()<n.date())return-t(n,e);var r=12*(n.year()-e.year())+(n.month()-e.month()),i=e.clone().add(r,s),a=n-i<0,o=e.clone().add(r+(a?-1:1),s);return+(-(r+(n-i)/(a?i-o:o-i))||0)},a:function(t){return t<0?Math.ceil(t)||0:Math.floor(t)},p:function(t){return({M:s,y:u,w:a,d:"day",D:l,h:i,m:r,s:n,ms:e,Q:o})[t]||String(t||"").toLowerCase().replace(/s$/,"")},u:function(t){return void 0===t}}).l=$,D.i=g,D.w=function(t,e){return y(t,{locale:e.$L,utc:e.$u,x:e.$x,$offset:e.$offset})},w=(v=function(){function t(t){this.$L=$(t.locale,null,!0),this.parse(t)}var f=t.prototype;return f.parse=function(t){this.$d=function(t){var e=t.date,n=t.utc;if(null===e)return new Date(NaN);if(D.u(e))return new Date;if(e instanceof Date)return new Date(e);if("string"==typeof e&&!/Z$/i.test(e)){var r=e.match(d);if(r){var i=r[2]-1||0,a=(r[7]||"0").substring(0,3);return n?new Date(Date.UTC(r[1],i,r[3]||1,r[4]||0,r[5]||0,r[6]||0,a)):new Date(r[1],i,r[3]||1,r[4]||0,r[5]||0,r[6]||0,a)}}return new Date(e)}(t),this.$x=t.x||{},this.init()},f.init=function(){var t=this.$d;this.$y=t.getFullYear(),this.$M=t.getMonth(),this.$D=t.getDate(),this.$W=t.getDay(),this.$H=t.getHours(),this.$m=t.getMinutes(),this.$s=t.getSeconds(),this.$ms=t.getMilliseconds()},f.$utils=function(){return D},f.isValid=function(){return this.$d.toString()!==c},f.isSame=function(t,e){var n=y(t);return this.startOf(e)<=n&&n<=this.endOf(e)},f.isAfter=function(t,e){return y(t)<this.startOf(e)},f.isBefore=function(t,e){return this.endOf(e)<y(t)},f.$g=function(t,e,n){return D.u(t)?this[e]:this.set(n,t)},f.unix=function(){return Math.floor(this.valueOf()/1e3)},f.valueOf=function(){return this.$d.getTime()},f.startOf=function(t,e){var o=this,c=!!D.u(e)||e,d=D.p(t),h=function(t,e){var n=D.w(o.$u?Date.UTC(o.$y,e,t):new Date(o.$y,e,t),o);return c?n:n.endOf("day")},f=function(t,e){return D.w(o.toDate()[t].apply(o.toDate("s"),(c?[0,0,0,0]:[23,59,59,999]).slice(e)),o)},m=this.$W,p=this.$M,g=this.$D,$="set"+(this.$u?"UTC":"");switch(d){case u:return c?h(1,0):h(31,11);case s:return c?h(1,p):h(0,p+1);case a:var y=this.$locale().weekStart||0,v=(m<y?m+7:m)-y;return h(c?g-v:g+(6-v),p);case"day":case l:return f($+"Hours",0);case i:return f($+"Minutes",1);case r:return f($+"Seconds",2);case n:return f($+"Milliseconds",3);default:return this.clone()}},f.endOf=function(t){return this.startOf(t,!1)},f.$set=function(t,a){var o,c=D.p(t),d="set"+(this.$u?"UTC":""),h=((o={}).day=d+"Date",o[l]=d+"Date",o[s]=d+"Month",o[u]=d+"FullYear",o[i]=d+"Hours",o[r]=d+"Minutes",o[n]=d+"Seconds",o[e]=d+"Milliseconds",o)[c],f="day"===c?this.$D+(a-this.$W):a;if(c===s||c===u){var m=this.clone().set(l,1);m.$d[h](f),m.init(),this.$d=m.set(l,Math.min(this.$D,m.daysInMonth())).$d}else h&&this.$d[h](f);return this.init(),this},f.set=function(t,e){return this.clone().$set(t,e)},f.get=function(t){return this[D.p(t)]()},f.add=function(t,e){var o,l=this;t=Number(t);var c=D.p(e),d=function(e){var n=y(l);return D.w(n.date(n.date()+Math.round(e*t)),l)};if(c===s)return this.set(s,this.$M+t);if(c===u)return this.set(u,this.$y+t);if("day"===c)return d(1);if(c===a)return d(7);var h=((o={})[r]=6e4,o[i]=36e5,o[n]=1e3,o)[c]||1,f=this.$d.getTime()+t*h;return D.w(f,this)},f.subtract=function(t,e){return this.add(-1*t,e)},f.format=function(t){var e=this,n=this.$locale();if(!this.isValid())return n.invalidDate||c;var r=t||"YYYY-MM-DDTHH:mm:ssZ",i=D.z(this),a=this.$H,s=this.$m,o=this.$M,u=n.weekdays,l=n.months,d=function(t,n,i,a){return t&&(t[n]||t(e,r))||i[n].slice(0,a)},f=function(t){return D.s(a%12||12,t,"0")},m=n.meridiem||function(t,e,n){var r=t<12?"AM":"PM";return n?r.toLowerCase():r},p={YY:String(this.$y).slice(-2),YYYY:D.s(this.$y,4,"0"),M:o+1,MM:D.s(o+1,2,"0"),MMM:d(n.monthsShort,o,l,3),MMMM:d(l,o),D:this.$D,DD:D.s(this.$D,2,"0"),d:String(this.$W),dd:d(n.weekdaysMin,this.$W,u,2),ddd:d(n.weekdaysShort,this.$W,u,3),dddd:u[this.$W],H:String(a),HH:D.s(a,2,"0"),h:f(1),hh:f(2),a:m(a,s,!0),A:m(a,s,!1),m:String(s),mm:D.s(s,2,"0"),s:String(this.$s),ss:D.s(this.$s,2,"0"),SSS:D.s(this.$ms,3,"0"),Z:i};return r.replace(h,function(t,e){return e||p[t]||i.replace(":","")})},f.utcOffset=function(){return-(15*Math.round(this.$d.getTimezoneOffset()/15))},f.diff=function(t,e,l){var c,d=D.p(e),h=y(t),f=(h.utcOffset()-this.utcOffset())*6e4,m=this-h,p=D.m(this,h);return p=((c={})[u]=p/12,c[s]=p,c[o]=p/3,c[a]=(m-f)/6048e5,c.day=(m-f)/864e5,c[i]=m/36e5,c[r]=m/6e4,c[n]=m/1e3,c)[d]||m,l?p:D.a(p)},f.daysInMonth=function(){return this.endOf(s).$D},f.$locale=function(){return p[this.$L]},f.locale=function(t,e){if(!t)return this.$L;var n=this.clone(),r=$(t,e,!0);return r&&(n.$L=r),n},f.clone=function(){return D.w(this.$d,this)},f.toDate=function(){return new Date(this.valueOf())},f.toJSON=function(){return this.isValid()?this.toISOString():null},f.toISOString=function(){return this.$d.toISOString()},f.toString=function(){return this.$d.toUTCString()},t}()).prototype,y.prototype=w,[["$ms",e],["$s",n],["$m",r],["$H",i],["$W","day"],["$M",s],["$y",u],["$D",l]].forEach(function(t){w[t[1]]=function(e){return this.$g(e,t[0],t[1])}}),y.extend=function(t,e){return t.$i||(t(e,v,y),t.$i=!0),y},y.locale=$,y.isDayjs=g,y.unix=function(t){return y(1e3*t)},y.en=p[m],y.Ls=p,y.p={},y)},6230:function(t){t.exports="object"==typeof self?self.FormData:window.FormData},8557:function(t,e,n){(window.__NEXT_P=window.__NEXT_P||[]).push(["/dashboard/calendar/create",function(){return n(3704)}])},85825:function(t,e,n){"use strict";var r=n(85893),i=n(17064);let a=t=>{let{className:e,onClick:n,children:a,type:s,variant:o="contained",loading:u}=t;return(0,r.jsx)(i.E.button,{whileTap:{y:1.5},onClick:n,type:s,className:"rounded-md capitalize p-2  min-w-[100px] hover:bg-[#009ABC] flex justify-center items ".concat("outlined"===o?"bg-transparent text-textColor hover:bg-slate-100 border-2":"text-white bg-highlight"," ").concat(e),children:u?(0,r.jsx)("div",{className:"loader-async"}):a})};e.Z=a},99537:function(t,e,n){"use strict";var r=n(85893);let i=t=>{let{className:e,children:n}=t;return(0,r.jsx)("div",{className:"bg-[#ffffff] rounded-md shadow-sm w-full p-4 ".concat(e),children:n})};e.Z=i},29186:function(t,e,n){"use strict";var r=n(85893);let i=t=>{let{label:e,type:n,value:i,name:a,onChange:s,error:o,errorMessage:u,id:l,multiple:c}=t;return(0,r.jsxs)("div",{children:[(0,r.jsxs)("div",{className:"relative  group min-w-xs ",children:[(0,r.jsx)("label",{htmlFor:"",className:"absolute left-3  top-1/2\n        -translate-y-1/2 \n      \n        group-focus-within:bg-white\n        group-focus-within:translate-y-[-200%]\n        group-focus-within:px-1 \n        group-focus-within:text-highlight transition-transform pointer-events-none\n         leading-none\n        group-focus-within:scale-90   duration-300 text-[#BDC0CC] ".concat(i?"translate-y-[-200%]  text-highlight  scale-90 px-1 bg-white":""," ").concat(o?" text-red-400":""),children:e}),(0,r.jsx)("input",{type:n,value:i,name:a,className:"border-[1px] border-slate-300 rounded-md w-full p-2 outline-highlight hover:border-slate-800  bg-transparent focus:border-highlight focus:outline-none ".concat(o?"border border-red-400":""),onChange:s,autoComplete:"new-password",id:l,multiple:c})]}),o?(0,r.jsx)("p",{className:"text-xs text-red-400 mt-1",children:u}):null]})};e.Z=i},90197:function(t,e,n){"use strict";var r=n(85893);n(67294);var i=n(9198),a=n.n(i);let s=t=>{let{value:e,onChange:n,name:i,inline:s,placeholderText:o,showTimeSelect:u,dateFormat:l}=t;return(0,r.jsx)(a(),{selected:e,onChange:n,className:"border-[1px] border-slate-300 rounded-lg w-full p-2 outline-highlight hover:border-slate-800 bg-transparent z-100",placeholderText:o||"Click to select date",name:i,inline:s,showTimeSelect:u,showIcon:!0,dateFormat:l})};e.Z=s},3704:function(t,e,n){"use strict";n.r(e),n.d(e,{default:function(){return y}});var r=n(85893),i=n(85825),a=n(29186),s=n(99537),o=n(53641),u=n(48228),l=n(67294),c=n(25192),d=n(6230),h=n.n(d),f=n(27484),m=n.n(f);let p=()=>{let[t,e]=(0,l.useState)({summary:"",location:"",description:"",startDateTime:new Date,endDateTime:new Date,AttendeeEmail:"",popopReminder:""}),n=new(h()),r=(0,u.D)(()=>{let e=t.AttendeeEmail.split(",");return n.append("Summary",t.summary),n.append("Description",t.description),e.forEach(t=>{n.append("AttendeeEmail",t)}),n.append("StartDateTime",m()(t.startDateTime.toISOString()).format("YYYY-MM-DDTHH:mm")),n.append("Location","Lagos"),n.append("EndDateTime",m()(t.endDateTime.toISOString()).format("YYYY-MM-DDTHH:mm")),n.append("EmailReminderTime","2"),n.append("PopUpReminderTime","50"),o.p.request({url:"/Dashboard/Calendar/Create",method:"POST",data:n,headers:{"Content-Type":"multipart/form-data"}})},{onSuccess:()=>{(0,c.C)("Done"),e({summary:"",location:"",description:"",startDateTime:new Date,endDateTime:new Date,AttendeeEmail:"",popopReminder:""})}}),i=(t,n)=>e(e=>({...e,[n]:t})),a=r.isLoading;return{calendarDetails:t,handleChange:function(t){let n=t.target.name,r=t.target.value;e(t=>({...t,[n]:r}))},handleDateChange:i,handleSubmit:function(t){t.preventDefault(),r.mutate()},loading:a}};var g=n(90197);let $=()=>{let{handleChange:t,calendarDetails:e,handleDateChange:n,handleSubmit:o,loading:u}=p();return(0,r.jsx)(r.Fragment,{children:(0,r.jsxs)(s.Z,{className:" p-8 mt-8 max-w-md mx-auto",children:[(0,r.jsx)("h3",{className:"mb-3 text-highlight",children:"Create Calendar Event"}),(0,r.jsxs)("form",{action:"",className:"w-full space-y-6 ",children:[(0,r.jsx)(a.Z,{label:"Summary",name:"summary",onChange:t,value:e.summary}),(0,r.jsx)(a.Z,{label:"Description",name:"description",onChange:t,value:e.description}),(0,r.jsx)(a.Z,{label:"Email",multiple:!0,name:"AttendeeEmail",onChange:t,value:e.AttendeeEmail}),(0,r.jsx)(g.Z,{onChange:t=>n(t,"startDateTime"),placeholderText:"Start Date and Time",value:e.startDateTime,showTimeSelect:!0,dateFormat:"MMMM d, yyyy h:mm aa"}),(0,r.jsx)(g.Z,{onChange:t=>n(t,"endDateTime"),placeholderText:"End Date and Time",showTimeSelect:!0,value:e.endDateTime,dateFormat:"MMMM d, yyyy h:mm aa"}),(0,r.jsx)(i.Z,{onClick:o,loading:u,className:"w-full",children:"Create Event"})]})]})})};var y=$},25192:function(t,e,n){"use strict";n.d(e,{C:function(){return l}});var r=n(85893),i=n(86455),a=n.n(i),s=n(77630),o=n.n(s);let u=o()(a()),l=t=>u.fire({title:(0,r.jsx)("strong",{children:"Successful"}),html:(0,r.jsx)("i",{children:t}),icon:"success"})}},function(t){t.O(0,[711,198,774,888,179],function(){return t(t.s=8557)}),_N_E=t.O()}]);