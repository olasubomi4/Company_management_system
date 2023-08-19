(self.webpackChunk_N_E=self.webpackChunk_N_E||[]).push([[865],{6230:function(e){e.exports="object"==typeof self?self.FormData:window.FormData},46953:function(e,t,i){(window.__NEXT_P=window.__NEXT_P||[]).push(["/dashboard/biographys",function(){return i(32296)}])},85825:function(e,t,i){"use strict";var s=i(85893),a=i(17064);let n=e=>{let{className:t,onClick:i,children:n,type:l,variant:r="contained",loading:o}=e;return(0,s.jsx)(a.E.button,{whileTap:{y:1.5},onClick:i,type:l,className:"rounded-md capitalize p-2  min-w-[100px] hover:bg-[#009ABC] flex justify-center items ".concat("outlined"===r?"bg-transparent text-textColor hover:bg-slate-100 border-2":"text-white bg-highlight"," ").concat(t),children:o?(0,s.jsx)("div",{className:"loader-async"}):n})};t.Z=n},99537:function(e,t,i){"use strict";var s=i(85893);let a=e=>{let{className:t,children:i}=e;return(0,s.jsx)("div",{className:"bg-[#ffffff] rounded-md shadow-sm w-full p-4 ".concat(t),children:i})};t.Z=a},27090:function(e,t,i){"use strict";var s=i(85893),a=i(86893),n=i(85825);let l=e=>{let{type:t,name:i,confirmDelete:l,closeModal:r}=e;return(0,s.jsxs)("div",{className:" text-center w-full space-y-5",children:[(0,s.jsx)("div",{className:"grid place-items-center",children:(0,s.jsx)(a.Ybf,{className:"text-red-600 text-center border-2 border-red-500 p-2 rounded-full ",size:"70px"})}),(0,s.jsxs)("h4",{className:"text-2xl font-semibold ",children:["You are about to delete this ",t]}),(0,s.jsxs)("p",{children:[(0,s.jsxs)("span",{className:"font-medium text-highlight mr-2 capitalize ",children:[i," ",t]}),"will be deleted permamently",(0,s.jsx)("br",{})," Are you sure?"]}),(0,s.jsxs)("div",{className:"flex gap-7 items-center justify-end",children:[(0,s.jsx)(n.Z,{variant:"outlined",onClick:r,children:"Cancel"}),(0,s.jsx)(n.Z,{className:"bg-red-500 hover:bg-red-400",onClick:l,children:"Delete"})]})]})};t.Z=l},29186:function(e,t,i){"use strict";var s=i(85893);let a=e=>{let{label:t,type:i,value:a,name:n,onChange:l,error:r,errorMessage:o,id:c}=e;return(0,s.jsxs)("div",{children:[(0,s.jsxs)("div",{className:"relative  group min-w-xs ",children:[(0,s.jsx)("label",{htmlFor:"",className:"absolute left-3  top-1/2\n        -translate-y-1/2 \n      \n        group-focus-within:bg-white\n        group-focus-within:translate-y-[-200%]\n        group-focus-within:px-1 \n        group-focus-within:text-highlight transition-transform pointer-events-none\n         leading-none\n        group-focus-within:scale-90   duration-300 text-[#BDC0CC] ".concat(a?"translate-y-[-200%]  text-highlight  scale-90 px-1 bg-white":""," ").concat(r?" text-red-400":""),children:t}),(0,s.jsx)("input",{type:i,value:a,name:n,className:"border-[1px] border-slate-300 rounded-md w-full p-2 outline-highlight hover:border-slate-800  bg-transparent focus:border-highlight focus:outline-none ".concat(r?"border border-red-400":""),onChange:l,autoComplete:"new-password",id:c})]}),r?(0,s.jsx)("p",{className:"text-xs text-red-400 mt-1",children:o}):null]})};t.Z=a},50562:function(e,t,i){"use strict";var s=i(48228),a=i(53641),n=i(25192);let l=(e,t,i)=>{let l=(0,s.D)(()=>a.p.request({method:"delete",url:e}),{onSuccess:()=>{(0,n.C)("".concat(t," deleted successfully")),i()}});return l};t.Z=l},32296:function(e,t,i){"use strict";i.r(t),i.d(t,{default:function(){return C}});var s=i(85893),a=i(25675),n=i.n(a),l=i(99537),r=i(85825);let o=e=>{let{src:t,alt:i,title:a,name:o,handleClick:c}=e;return(0,s.jsxs)(l.Z,{className:"text-center max-w-xs space-y-4",children:[(0,s.jsx)(n(),{src:t,alt:i,className:"rounded-full"}),(0,s.jsx)("p",{className:"text-xl text-highlight font-medium",children:o}),(0,s.jsx)("p",{className:"font-semibold",children:a}),(0,s.jsx)(r.Z,{className:"w-full",onClick:c,children:"View Full Profile"})]})};var c=i(29186),d=i(55260),u=i(27090),m=i(67848),h=i(53641),p=i(85945),x=i(48228),f=i(67294),g=i(25192),j=i(6230),N=i.n(j),b=i(50562);let v=()=>{let e=new(N()),[t,i]=(0,f.useState)({firstName:"",lastName:"",position:"",image:""}),[s,a]=(0,f.useState)(!1),[n,l]=(0,f.useState)(!1),[r,o]=(0,f.useState)(""),c=(0,b.Z)("/Dashboard/Admin/Biography/Delete?id=".concat(r.id),"Biography",()=>{a(!1),l(!1),u.invalidateQueries(["biographies"])}),d=()=>{c.mutate()},u=(0,p.NL)(),m=(0,x.D)(async()=>{e.append("Biography.FirstName",t.firstName),e.append("Biography.LastName",t.lastName),e.append("Biography.Position",t.position),e.append("ProfileImage",t.image),e.append("ProfileVideo",t.image);let i=await h.p.request({url:"/Dashboard/Admin/Biography/Upsert",method:"post",data:e,headers:{"Content-Type":"multipart/form-data"}});return i},{onSuccess:()=>{(0,g.C)("Biography Created successfuly"),u.invalidateQueries(["biographies"]),l(!1),i({firstName:"",lastName:"",position:"",image:""})}});return{biographyData:t,setBiographyData:i,handleChange:function(e){let t=e.target.name,s=e.target.value;i(e=>({...e,[t]:s}))},handleSubmit:function(e){e.preventDefault(),m.mutate()},mutateBiography:m,setShowBiographyModal:l,showBiographyModal:n,showDeleteModal:s,setGetTableRow:o,confirmDelete:d,setShowDeleteModal:a,tableRow:r}},y=()=>{let{data:e}=(0,m.a)(["biographies"],()=>h.p.request({method:"get",url:"/Dashboard/Biography/GetAll"})),t=null==e?void 0:e.data,[i,a]=(0,f.useState)(!1),{biographyData:p,setBiographyData:x,handleChange:g,handleSubmit:j,mutateBiography:N,setShowBiographyModal:b,showBiographyModal:y,showDeleteModal:C,setGetTableRow:w,confirmDelete:Z,setShowDeleteModal:k,tableRow:S}=v();return(0,s.jsxs)(s.Fragment,{children:[(0,s.jsxs)("div",{className:"my-8",children:[(0,s.jsx)(l.Z,{className:"my-3",children:(0,s.jsx)("div",{className:"flex items-center justify-end",children:(0,s.jsx)(r.Z,{onClick:()=>b(!0),children:"Add Biography"})})}),(0,s.jsx)("div",{className:"flex gap-4 flex-wrap",children:null==t?void 0:t.map(e=>(0,s.jsx)(o,{src:JSON.parse(e.profileImageUrl)[0],alt:e.firstName,name:e.firstName+" "+e.lastName,title:e.position,handleClick:()=>{a(!0),w(e)}},e.id))})]}),(0,s.jsx)(d.Z,{openState:y,onClick:()=>b(!1),children:(0,s.jsxs)("div",{className:"",children:[(0,s.jsx)("h3",{className:"text-xl font-medium",children:"Add Biography"}),(0,s.jsxs)("form",{action:"",onSubmit:j,autoComplete:"off",children:[(0,s.jsxs)("div",{className:" my-5 grid grid-cols-200 gap-5",children:[(0,s.jsx)(c.Z,{label:"First Name",name:"firstName",onChange:g,value:p.firstName,type:"text"}),(0,s.jsx)(c.Z,{label:"Last Name",name:"lastName",onChange:g,value:p.lastName,type:"text"})," ",(0,s.jsx)(c.Z,{label:"Position",name:"position",onChange:g,value:p.position,type:"text"})," ",(0,s.jsxs)("div",{className:"",children:[(0,s.jsx)("label",{htmlFor:"profile-image",className:"block text-center border border-slate-300 rounded-lg w-full p-2 bg-[#ADD8E6] text-white",children:p.image?p.image.name:"Choose Profile Picture"}),(0,s.jsx)("input",{type:"file",name:"",id:"profile-image",className:"hidden",onChange:e=>x(t=>({...t,image:e.target.files[0]})),accept:"image/*"})]})]}),(0,s.jsxs)("div",{className:"mt-3 flex items-center justify-end gap-4",children:[(0,s.jsx)(r.Z,{variant:"outlined",onClick:()=>b(!1),children:"Cancel"}),(0,s.jsx)(r.Z,{onClick:e=>{j(e),N.isSuccess&&b(!1)},loading:N.isLoading,children:"Submit"})]})]})]})}),(0,s.jsx)(d.Z,{openState:i,onClick:()=>a(!1),children:(0,s.jsxs)("div",{className:"",children:[(0,s.jsx)("h3",{className:"text-xl font-medium",children:S.firstName}),(0,s.jsxs)("div",{className:" my-5 grid grid-cols-240 gap-2 ",children:[(0,s.jsx)("div",{className:"",children:(0,s.jsx)(n(),{src:JSON.parse(S.profileImageUrl)[0],alt:"profile"})}),(0,s.jsx)("p",{children:"Lorem ipsum dolor sit amet consectetur adipisicing elit. Reiciendis, ad. Aperiam odio ducimus impedit harum neque amet ipsam eligendi autem aliquam unde qui ullam repudiandae, corporis hic veritatis, nisi minima, tenetur accusamus beatae deleniti sed! Dignissimos debitis soluta quae sint."})]}),(0,s.jsxs)("div",{className:"mt-3 flex items-center justify-end gap-4",children:[(0,s.jsx)(r.Z,{variant:"outlined",onClick:()=>a(!1),children:"Cancel"}),(0,s.jsx)(r.Z,{onClick:()=>k(!0),className:"bg-red-500 hover:bg-red-400",children:"Delete"})]})]})}),(0,s.jsx)(d.Z,{openState:C,onClick:()=>k(!1),children:(0,s.jsx)(u.Z,{name:null==S?void 0:S.name,type:"biography",confirmDelete:Z,closeModal:()=>k(!1)})})]})};var C=y},25192:function(e,t,i){"use strict";i.d(t,{C:function(){return c}});var s=i(85893),a=i(86455),n=i.n(a),l=i(77630),r=i.n(l);let o=r()(n()),c=e=>o.fire({title:(0,s.jsx)("strong",{children:"Successful"}),html:(0,s.jsx)("i",{children:e}),icon:"success"})}},function(e){e.O(0,[711,774,888,179],function(){return e(e.s=46953)}),_N_E=e.O()}]);