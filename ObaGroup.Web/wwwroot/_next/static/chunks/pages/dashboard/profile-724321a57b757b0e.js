(self.webpackChunk_N_E=self.webpackChunk_N_E||[]).push([[389],{50540:function(e,t,l){(window.__NEXT_P=window.__NEXT_P||[]).push(["/dashboard/profile",function(){return l(65357)}])},85825:function(e,t,l){"use strict";var n=l(85893),r=l(17064);let a=e=>{let{className:t,onClick:l,children:a,type:s,variant:c="contained",loading:i}=e;return(0,n.jsx)(r.E.button,{whileTap:{y:1.5},onClick:l,type:s,className:"rounded-md capitalize p-2  min-w-[100px] hover:bg-[#009ABC] flex justify-center items ".concat("outlined"===c?"bg-transparent text-textColor hover:bg-slate-100 border-2":"text-white bg-highlight"," ").concat(t),children:i?(0,n.jsx)("div",{className:"loader-async"}):a})};t.Z=a},99537:function(e,t,l){"use strict";var n=l(85893);let r=e=>{let{className:t,children:l}=e;return(0,n.jsx)("div",{className:"bg-[#ffffff] rounded-md shadow-sm w-full p-4 ".concat(t),children:l})};t.Z=r},29186:function(e,t,l){"use strict";var n=l(85893);let r=e=>{let{label:t,type:l,value:r,name:a,onChange:s,error:c,errorMessage:i,id:o}=e;return(0,n.jsxs)("div",{children:[(0,n.jsxs)("div",{className:"relative  group min-w-xs ",children:[(0,n.jsx)("label",{htmlFor:"",className:"absolute left-3  top-1/2\n        -translate-y-1/2 \n      \n        group-focus-within:bg-white\n        group-focus-within:translate-y-[-200%]\n        group-focus-within:px-1 \n        group-focus-within:text-highlight transition-transform pointer-events-none\n         leading-none\n        group-focus-within:scale-90   duration-300 text-[#BDC0CC] ".concat(r?"translate-y-[-200%]  text-highlight  scale-90 px-1 bg-white":""," ").concat(c?" text-red-400":""),children:t}),(0,n.jsx)("input",{type:l,value:r,name:a,className:"border-[1px] border-slate-300 rounded-md w-full p-2 outline-highlight hover:border-slate-800  bg-transparent focus:border-highlight focus:outline-none ".concat(c?"border border-red-400":""),onChange:s,autoComplete:"new-password",id:o})]}),c?(0,n.jsx)("p",{className:"text-xs text-red-400 mt-1",children:i}):null]})};t.Z=r},65357:function(e,t,l){"use strict";let n;l.r(t),l.d(t,{default:function(){return D}});var r=l(85893),a=l(25675),s=l.n(a),c=l(86893),i=l(67294);function o(e){return t=>!!t.type&&t.type.tabsRole===e}let d=o("Tab"),u=o("TabList"),f=o("TabPanel");function p(e,t){return i.Children.map(e,e=>null===e?null:d(e)||u(e)||f(e)?t(e):e.props&&e.props.children&&"object"==typeof e.props.children?(0,i.cloneElement)(e,{...e.props,children:p(e.props.children,t)}):e)}var m=l(86010);function h(e){let t=0;return!function e(t,l){return i.Children.forEach(t,t=>{null!==t&&(d(t)||f(t)?l(t):t.props&&t.props.children&&"object"==typeof t.props.children&&(u(t)&&l(t),e(t.props.children,l)))})}(e,e=>{d(e)&&t++}),t}function b(e){return e&&"getAttribute"in e}function x(e){return b(e)&&e.getAttribute("data-rttab")}function N(e){return b(e)&&"true"===e.getAttribute("aria-disabled")}let g={className:"react-tabs",focus:!1},w=e=>{let t=(0,i.useRef)([]),l=(0,i.useRef)([]),r=(0,i.useRef)();function a(t,l){if(t<0||t>=o())return;let{onSelect:n,selectedIndex:r}=e;n(t,r,l)}function s(e){let t=o();for(let l=e+1;l<t;l++)if(!N(b(l)))return l;for(let t=0;t<e;t++)if(!N(b(t)))return t;return e}function c(e){let t=e;for(;t--;)if(!N(b(t)))return t;for(t=o();t-- >e;)if(!N(b(t)))return t;return e}function o(){let{children:t}=e;return h(t)}function b(e){return t.current[`tabs-${e}`]}function w(e){let t=e.target;do if(j(t)){if(N(t))return;let l=[].slice.call(t.parentNode.children).filter(x).indexOf(t);a(l,e);return}while(null!=(t=t.parentNode))}function j(e){if(!x(e))return!1;let t=e.parentElement;do{if(t===r.current)return!0;if(t.getAttribute("data-rttabs"))break;t=t.parentElement}while(t);return!1}let{children:y,className:A,disabledTabClassName:v,domRef:C,focus:E,forceRenderTabPanel:T,onSelect:R,selectedIndex:k,selectedTabClassName:_,selectedTabPanelClassName:P,environment:O,disableUpDownKeys:Z,disableLeftRightKeys:S,...I}={...g,...e};return i.createElement("div",Object.assign({},I,{className:(0,m.default)(A),onClick:w,onKeyDown:function(t){let{direction:l,disableUpDownKeys:n,disableLeftRightKeys:r}=e;if(j(t.target)){let{selectedIndex:i}=e,d=!1,u=!1;("Space"===t.code||32===t.keyCode||"Enter"===t.code||13===t.keyCode)&&(d=!0,u=!1,w(t)),(r||37!==t.keyCode&&"ArrowLeft"!==t.code)&&(n||38!==t.keyCode&&"ArrowUp"!==t.code)?(r||39!==t.keyCode&&"ArrowRight"!==t.code)&&(n||40!==t.keyCode&&"ArrowDown"!==t.code)?35===t.keyCode||"End"===t.code?(i=function(){let e=o();for(;e--;)if(!N(b(e)))return e;return null}(),d=!0,u=!0):(36===t.keyCode||"Home"===t.code)&&(i=function(){let e=o();for(let t=0;t<e;t++)if(!N(b(t)))return t;return null}(),d=!0,u=!0):(i="rtl"===l?c(i):s(i),d=!0,u=!0):(i="rtl"===l?s(i):c(i),d=!0,u=!0),d&&t.preventDefault(),u&&a(i,t)}},ref:e=>{r.current=e,C&&C(e)},"data-rttabs":!0}),function(){let r=0,{children:a,disabledTabClassName:s,focus:c,forceRenderTabPanel:m,selectedIndex:h,selectedTabClassName:x,selectedTabPanelClassName:N,environment:g}=e;l.current=l.current||[];let w=l.current.length-o(),j=(0,i.useId)();for(;w++<0;)l.current.push(`${j}${l.current.length}`);return p(a,e=>{let a=e;if(u(e)){let r=0,o=!1;null==n&&function(e){let t=e||("undefined"!=typeof window?window:void 0);try{n=!!(void 0!==t&&t.document&&t.document.activeElement)}catch(e){n=!1}}(g);let u=g||("undefined"!=typeof window?window:void 0);n&&u&&(o=i.Children.toArray(e.props.children).filter(d).some((e,t)=>u.document.activeElement===b(t))),a=(0,i.cloneElement)(e,{children:p(e.props.children,e=>{let n=`tabs-${r}`,a=h===r,d={tabRef:e=>{t.current[n]=e},id:l.current[r],selected:a,focus:a&&(c||o)};return x&&(d.selectedClassName=x),s&&(d.disabledClassName=s),r++,(0,i.cloneElement)(e,d)})})}else if(f(e)){let t={id:l.current[r],selected:h===r};m&&(t.forceRender=m),N&&(t.selectedClassName=N),r++,a=(0,i.cloneElement)(e,t)}return a})}())};w.propTypes={};let j={defaultFocus:!1,focusTabOnClick:!0,forceRenderTabPanel:!1,selectedIndex:null,defaultIndex:null,environment:null,disableUpDownKeys:!1,disableLeftRightKeys:!1},y=e=>null===e.selectedIndex?1:0,A=(e,t)=>{},v=e=>{let{children:t,defaultFocus:l,defaultIndex:n,focusTabOnClick:r,onSelect:a,...s}={...j,...e},[c,o]=(0,i.useState)(l),[d]=(0,i.useState)(y(s)),[u,f]=(0,i.useState)(1===d?n||0:null);if((0,i.useEffect)(()=>{o(!1)},[]),1===d){let e=h(t);(0,i.useEffect)(()=>{null!=u&&f(Math.min(u,Math.max(0,e-1)))},[e])}A(s,d);let p=(e,t,l)=>{("function"!=typeof a||!1!==a(e,t,l))&&(r&&o(!0),1===d&&f(e))},m={...e,...s};return m.focus=c,m.onSelect=p,null!=u&&(m.selectedIndex=u),delete m.defaultFocus,delete m.defaultIndex,delete m.focusTabOnClick,i.createElement(w,m,t)};v.propTypes={},v.tabsRole="Tabs";let C={className:"react-tabs__tab-list"},E=e=>{let{children:t,className:l,...n}={...C,...e};return i.createElement("ul",Object.assign({},n,{className:(0,m.default)(l),role:"tablist"}),t)};E.tabsRole="TabList",E.propTypes={};let T="react-tabs__tab",R={className:T,disabledClassName:`${T}--disabled`,focus:!1,id:null,selected:!1,selectedClassName:`${T}--selected`},k=e=>{let t=(0,i.useRef)(),{children:l,className:n,disabled:r,disabledClassName:a,focus:s,id:c,selected:o,selectedClassName:d,tabIndex:u,tabRef:f,...p}={...R,...e};return(0,i.useEffect)(()=>{o&&s&&t.current.focus()},[o,s]),i.createElement("li",Object.assign({},p,{className:(0,m.default)(n,{[d]:o,[a]:r}),ref:e=>{t.current=e,f&&f(e)},role:"tab",id:`tab${c}`,"aria-selected":o?"true":"false","aria-disabled":r?"true":"false","aria-controls":`panel${c}`,tabIndex:u||(o?"0":null),"data-rttab":!0}),l)};k.propTypes={},k.tabsRole="Tab";let _="react-tabs__tab-panel",P={className:_,forceRender:!1,selectedClassName:`${_}--selected`},O=e=>{let{children:t,className:l,forceRender:n,id:r,selected:a,selectedClassName:s,...c}={...P,...e};return i.createElement("div",Object.assign({},c,{className:(0,m.default)(l,{[s]:a}),role:"tabpanel",id:`panel${r}`,"aria-labelledby":`tab${r}`}),n||a?t:null)};O.tabsRole="TabPanel",O.propTypes={};var Z={src:"/_next/static/media/avatar.c8d1cc3e.png",height:258,width:260,blurDataURL:"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAMAAADz0U65AAAASFBMVEX////+/v7+/v39/v76+vr5+fnt7e7s7e7q6+zn6Onn6Ojm5+jk5ebj5ebT1dbS1NbR09XR09TQ0tTP0dPNz9HMztHLzc/Kzc/BTOHiAAAAP0lEQVR42h3GSRKAIAwF0Y8GBWUIhOH+NyWV6kU/OFzPe8NpYe9gSGNkwzfnbyBupAM8s9dRrCI1EsqS3mWVAzvnAh4wmyWfAAAAAElFTkSuQmCC",blurWidth:8,blurHeight:8},S=l(29186),I=l(85825),$=l(99537),z=l(40591);let U=()=>{let{user:e}=(0,z.Z)(e=>e.user);return(0,r.jsx)("div",{className:"mt-8",children:(0,r.jsxs)(v,{children:[(0,r.jsxs)(E,{className:"flex justify-between gap-8 items-center border-b border-slate-400",children:[(0,r.jsxs)(k,{className:"flex gap-2 items-center text-gray-400 py-2 px-4 cursor-pointer",selectedClassName:"active-tab",children:[(0,r.jsx)(c.Nte,{})," Edit Profile"]}),(0,r.jsxs)(k,{className:"flex gap-2 items-center text-gray-400 py-2 px-4 cursor-pointer",selectedClassName:"active-tab",children:[(0,r.jsx)(c.UIZ,{})," Security"]}),(0,r.jsxs)(k,{className:"flex gap-2 items-center text-gray-400 py-2 px-4 cursor-pointer",selectedClassName:"active-tab",children:[(0,r.jsx)(c.nbt,{})," Appearance"]}),(0,r.jsxs)(k,{className:"flex gap-2 items-center text-gray-400 py-2 px-4 cursor-pointer",selectedClassName:"active-tab",children:[(0,r.jsx)(c.bax,{})," Help"]})]}),(0,r.jsx)(O,{children:(0,r.jsxs)($.Z,{className:" my-4 px-8",children:[(0,r.jsxs)("div",{className:"flex justify-between items-center md:max-w-lg mx-auto",children:[(0,r.jsx)("h3",{className:"text-2xl font-semibold",children:"Edit Profile"}),(0,r.jsx)("div",{className:"rounded-full w-12 h-12",children:(0,r.jsx)(s(),{src:Z,alt:"profile picture",className:"w-12 h-12 rounded-full "})})]}),(0,r.jsxs)("form",{action:"",className:"space-y-6 my-4 md:max-w-lg mx-auto",children:[(0,r.jsx)(S.Z,{label:"First Name",value:null==e?void 0:e.firstName}),(0,r.jsx)(S.Z,{label:"Last Name",name:"lastName",value:null==e?void 0:e.lastName}),(0,r.jsx)(S.Z,{label:"Department",name:"position"}),(0,r.jsx)(S.Z,{label:"Phone No"}),(0,r.jsx)(I.Z,{className:"w-full",children:"Save Changes"})]})]})}),(0,r.jsx)(O,{className:"my-4",children:(0,r.jsx)($.Z,{className:"  p-7 h-full",children:(0,r.jsxs)("div",{className:"max-w-lg mx-auto space-y-4",children:[(0,r.jsx)("h3",{className:"text-xl text-center",children:"Change Your Password"}),(0,r.jsxs)("form",{action:"",className:"space-y-4",children:[(0,r.jsx)(S.Z,{label:"Old Password"}),(0,r.jsx)(S.Z,{label:"New Password"}),(0,r.jsx)(S.Z,{label:"Confirm Password"}),(0,r.jsx)(I.Z,{className:"w-full",children:"Change Password"})]})]})})}),(0,r.jsx)(O,{children:(0,r.jsx)("h2",{children:"Any content 2"})}),(0,r.jsx)(O,{children:(0,r.jsx)("h2",{children:"Any content 1"})})]})})};var D=U}},function(e){e.O(0,[774,888,179],function(){return e(e.s=50540)}),_N_E=e.O()}]);