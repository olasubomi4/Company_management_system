(self.webpackChunk_N_E=self.webpackChunk_N_E||[]).push([[459],{64167:function(e,t,n){(window.__NEXT_P=window.__NEXT_P||[]).push(["/login",function(){return n(82191)}])},85825:function(e,t,n){"use strict";var s=n(85893),a=n(17064);let l=e=>{let{className:t,onClick:n,children:l,type:r,variant:i="contained"}=e;return(0,s.jsx)(a.E.button,{whileTap:{y:1.5},onClick:n,type:r,className:"rounded-md capitalize p-2  min-w-[100px] hover:bg-[#009ABC] ".concat("outlined"===i?"bg-transparent text-textColor hover:bg-slate-100 border-2":"text-white bg-highlight"," ").concat(t),children:l})};t.Z=l},29186:function(e,t,n){"use strict";var s=n(85893);let a=e=>{let{label:t,type:n,value:a,name:l,onChange:r,error:i,errorMessage:o,id:c}=e;return(0,s.jsxs)("div",{children:[(0,s.jsxs)("div",{className:"relative  group min-w-xs ",children:[(0,s.jsx)("label",{htmlFor:"",className:"absolute left-3  top-1/2\n        -translate-y-1/2 \n        group-focus-within:bg-white\n        group-focus-within:translate-y-[-200%]\n        group-focus-within:px-1 \n        group-focus-within:text-highlight transition-transform pointer-events-none\n         leading-none\n        group-focus-within:scale-90   duration-300 text-[#BDC0CC] ".concat(a?"translate-y-[-200%] text-highlight  scale-90 px-1 bg-white":""," ").concat(i?" text-red-400":""),children:t}),(0,s.jsx)("input",{type:n,value:a,name:l,className:"border-[1px] border-slate-300 rounded-md w-full p-2 outline-highlight hover:border-slate-800  bg-transparent focus:border-highlight focus:outline-none ".concat(i?"border border-red-400":""),onChange:r,autoComplete:"new-password",id:c})]}),i?(0,s.jsx)("p",{className:"text-xs text-red-400 mt-1",children:o}):null]})};t.Z=a},37472:function(e,t,n){"use strict";n.d(t,{Z:function(){return o}});var s=n(85893),a=n(25675),l=n.n(a),r={src:"/_next/static/media/login_man.100f069f.svg",height:552,width:793,blurWidth:0,blurHeight:0};let i=e=>{let{children:t}=e;return(0,s.jsx)("div",{className:"min-h-[100vh] grid place-items-center ",children:(0,s.jsxs)("div",{className:"grid md:grid-cols-2 place-items-center max-w-4xl gap-8 shadow-lg min-h-[500px] rounded-2xl py-2 px-4 bg-[#fcfcfc]",children:[(0,s.jsx)("div",{className:" hidden md:block",children:(0,s.jsx)(l(),{src:r,alt:"login_image"})}),(0,s.jsx)("div",{className:"h-full w-full grid place-items-center",children:(0,s.jsx)("form",{action:"",className:"w-full text-center ",children:t})})]})})};var o=i},82191:function(e,t,n){"use strict";n.r(t),n.d(t,{default:function(){return g}});var s=n(85893),a=n(85825),l=n(29186),r=n(41664),i=n.n(r),o=n(37472),c=n(53641),d=n(48228),u=n(11163),h=n(67294),p=n(71990);n(31955);let m=()=>{let e=(0,u.useRouter)(),t=new URLSearchParams,n=(0,d.D)(()=>(t.append("email",s.email),t.append("password",s.password),c.p.request({url:"login",method:"POST",data:t,headers:{"Content-Type":"application/x-www-form-urlencoded"}})),{onSuccess:()=>{(0,p.t5)("Login Successful"),e.push("/dashboard/upload")}}),[s,a]=(0,h.useState)({email:"",password:""});return[s.email,s.password,function(e){let t=e.target.name,n=e.target.value;a(e=>({...e,[t]:n}))},function(e){e.preventDefault(),n.mutate()}]},x=()=>{let[e,t,n,r]=m();return(0,s.jsxs)(o.Z,{children:[(0,s.jsx)("h2",{className:"text-4xl my-6 font-medium ",children:"Welcome"}),(0,s.jsx)("p",{className:"text-slate-600 mb-6",children:"Please provide your details to login to your account"}),(0,s.jsxs)("div",{className:"space-y-6",children:[(0,s.jsx)(l.Z,{label:"Email",name:"email",onChange:n,value:e}),(0,s.jsx)(l.Z,{label:"Password",name:"password",onChange:n,value:t,type:"password"})]}),(0,s.jsx)(a.Z,{className:"my-6 w-full",onClick:r,children:"Login"}),(0,s.jsxs)("div",{className:"uppercase my-4 flex justify-between items-center",children:[(0,s.jsx)("div",{className:"w-[40%] h-[1px] bg-slate-600"}),"or",(0,s.jsx)("div",{className:"w-[40%] h-[1px] bg-slate-600"})]}),(0,s.jsxs)("p",{children:["You dont have an account?",(0,s.jsx)(i(),{href:"/signup",className:"text-highlight mx-2",children:"Sign up"})]})]})};var g=x}},function(e){e.O(0,[774,888,179],function(){return e(e.s=64167)}),_N_E=e.O()}]);