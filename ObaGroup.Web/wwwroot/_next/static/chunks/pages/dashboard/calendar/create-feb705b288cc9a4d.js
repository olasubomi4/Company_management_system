(self.webpackChunk_N_E = self.webpackChunk_N_E || []).push([
  [187],
  {
    27484: function (t) {
      var e, n, r, i, s, a, o, u, c, l, d, h, f, m, p, $, g, y, v, D, w;
      t.exports =
        ((e = "millisecond"),
        (n = "second"),
        (r = "minute"),
        (i = "hour"),
        (s = "week"),
        (a = "month"),
        (o = "quarter"),
        (u = "year"),
        (c = "date"),
        (l = "Invalid Date"),
        (d =
          /^(\d{4})[-/]?(\d{1,2})?[-/]?(\d{0,2})[Tt\s]*(\d{1,2})?:?(\d{1,2})?:?(\d{1,2})?[.:]?(\d+)?$/),
        (h =
          /\[([^\]]+)]|Y{1,4}|M{1,4}|D{1,2}|d{1,4}|H{1,2}|h{1,2}|a|A|m{1,2}|s{1,2}|Z{1,2}|SSS/g),
        (f = function (t, e, n) {
          var r = String(t);
          return !r || r.length >= e
            ? t
            : "" + Array(e + 1 - r.length).join(n) + t;
        }),
        ((p = {})[(m = "en")] = {
          name: "en",
          weekdays:
            "Sunday_Monday_Tuesday_Wednesday_Thursday_Friday_Saturday".split(
              "_"
            ),
          months:
            "January_February_March_April_May_June_July_August_September_October_November_December".split(
              "_"
            ),
          ordinal: function (t) {
            var e = ["th", "st", "nd", "rd"],
              n = t % 100;
            return "[" + t + (e[(n - 20) % 10] || e[n] || "th") + "]";
          },
        }),
        ($ = function (t) {
          return t instanceof D;
        }),
        (g = function t(e, n, r) {
          var i;
          if (!e) return m;
          if ("string" == typeof e) {
            var s = e.toLowerCase();
            p[s] && (i = s), n && ((p[s] = n), (i = s));
            var a = e.split("-");
            if (!i && a.length > 1) return t(a[0]);
          } else {
            var o = e.name;
            (p[o] = e), (i = o);
          }
          return !r && i && (m = i), i || (!r && m);
        }),
        (y = function (t, e) {
          if ($(t)) return t.clone();
          var n = "object" == typeof e ? e : {};
          return (n.date = t), (n.args = arguments), new D(n);
        }),
        ((v = {
          s: f,
          z: function (t) {
            var e = -t.utcOffset(),
              n = Math.abs(e);
            return (
              (e <= 0 ? "+" : "-") +
              f(Math.floor(n / 60), 2, "0") +
              ":" +
              f(n % 60, 2, "0")
            );
          },
          m: function t(e, n) {
            if (e.date() < n.date()) return -t(n, e);
            var r = 12 * (n.year() - e.year()) + (n.month() - e.month()),
              i = e.clone().add(r, a),
              s = n - i < 0,
              o = e.clone().add(r + (s ? -1 : 1), a);
            return +(-(r + (n - i) / (s ? i - o : o - i)) || 0);
          },
          a: function (t) {
            return t < 0 ? Math.ceil(t) || 0 : Math.floor(t);
          },
          p: function (t) {
            return (
              {
                M: a,
                y: u,
                w: s,
                d: "day",
                D: c,
                h: i,
                m: r,
                s: n,
                ms: e,
                Q: o,
              }[t] ||
              String(t || "")
                .toLowerCase()
                .replace(/s$/, "")
            );
          },
          u: function (t) {
            return void 0 === t;
          },
        }).l = g),
        (v.i = $),
        (v.w = function (t, e) {
          return y(t, { locale: e.$L, utc: e.$u, x: e.$x, $offset: e.$offset });
        }),
        (w = (D = (function () {
          function t(t) {
            (this.$L = g(t.locale, null, !0)), this.parse(t);
          }
          var f = t.prototype;
          return (
            (f.parse = function (t) {
              (this.$d = (function (t) {
                var e = t.date,
                  n = t.utc;
                if (null === e) return new Date(NaN);
                if (v.u(e)) return new Date();
                if (e instanceof Date) return new Date(e);
                if ("string" == typeof e && !/Z$/i.test(e)) {
                  var r = e.match(d);
                  if (r) {
                    var i = r[2] - 1 || 0,
                      s = (r[7] || "0").substring(0, 3);
                    return n
                      ? new Date(
                          Date.UTC(
                            r[1],
                            i,
                            r[3] || 1,
                            r[4] || 0,
                            r[5] || 0,
                            r[6] || 0,
                            s
                          )
                        )
                      : new Date(
                          r[1],
                          i,
                          r[3] || 1,
                          r[4] || 0,
                          r[5] || 0,
                          r[6] || 0,
                          s
                        );
                  }
                }
                return new Date(e);
              })(t)),
                (this.$x = t.x || {}),
                this.init();
            }),
            (f.init = function () {
              var t = this.$d;
              (this.$y = t.getFullYear()),
                (this.$M = t.getMonth()),
                (this.$D = t.getDate()),
                (this.$W = t.getDay()),
                (this.$H = t.getHours()),
                (this.$m = t.getMinutes()),
                (this.$s = t.getSeconds()),
                (this.$ms = t.getMilliseconds());
            }),
            (f.$utils = function () {
              return v;
            }),
            (f.isValid = function () {
              return this.$d.toString() !== l;
            }),
            (f.isSame = function (t, e) {
              var n = y(t);
              return this.startOf(e) <= n && n <= this.endOf(e);
            }),
            (f.isAfter = function (t, e) {
              return y(t) < this.startOf(e);
            }),
            (f.isBefore = function (t, e) {
              return this.endOf(e) < y(t);
            }),
            (f.$g = function (t, e, n) {
              return v.u(t) ? this[e] : this.set(n, t);
            }),
            (f.unix = function () {
              return Math.floor(this.valueOf() / 1e3);
            }),
            (f.valueOf = function () {
              return this.$d.getTime();
            }),
            (f.startOf = function (t, e) {
              var o = this,
                l = !!v.u(e) || e,
                d = v.p(t),
                h = function (t, e) {
                  var n = v.w(
                    o.$u ? Date.UTC(o.$y, e, t) : new Date(o.$y, e, t),
                    o
                  );
                  return l ? n : n.endOf("day");
                },
                f = function (t, e) {
                  return v.w(
                    o
                      .toDate()
                      [t].apply(
                        o.toDate("s"),
                        (l ? [0, 0, 0, 0] : [23, 59, 59, 999]).slice(e)
                      ),
                    o
                  );
                },
                m = this.$W,
                p = this.$M,
                $ = this.$D,
                g = "set" + (this.$u ? "UTC" : "");
              switch (d) {
                case u:
                  return l ? h(1, 0) : h(31, 11);
                case a:
                  return l ? h(1, p) : h(0, p + 1);
                case s:
                  var y = this.$locale().weekStart || 0,
                    D = (m < y ? m + 7 : m) - y;
                  return h(l ? $ - D : $ + (6 - D), p);
                case "day":
                case c:
                  return f(g + "Hours", 0);
                case i:
                  return f(g + "Minutes", 1);
                case r:
                  return f(g + "Seconds", 2);
                case n:
                  return f(g + "Milliseconds", 3);
                default:
                  return this.clone();
              }
            }),
            (f.endOf = function (t) {
              return this.startOf(t, !1);
            }),
            (f.$set = function (t, s) {
              var o,
                l = v.p(t),
                d = "set" + (this.$u ? "UTC" : ""),
                h = (((o = {}).day = d + "Date"),
                (o[c] = d + "Date"),
                (o[a] = d + "Month"),
                (o[u] = d + "FullYear"),
                (o[i] = d + "Hours"),
                (o[r] = d + "Minutes"),
                (o[n] = d + "Seconds"),
                (o[e] = d + "Milliseconds"),
                o)[l],
                f = "day" === l ? this.$D + (s - this.$W) : s;
              if (l === a || l === u) {
                var m = this.clone().set(c, 1);
                m.$d[h](f),
                  m.init(),
                  (this.$d = m.set(c, Math.min(this.$D, m.daysInMonth())).$d);
              } else h && this.$d[h](f);
              return this.init(), this;
            }),
            (f.set = function (t, e) {
              return this.clone().$set(t, e);
            }),
            (f.get = function (t) {
              return this[v.p(t)]();
            }),
            (f.add = function (t, e) {
              var o,
                c = this;
              t = Number(t);
              var l = v.p(e),
                d = function (e) {
                  var n = y(c);
                  return v.w(n.date(n.date() + Math.round(e * t)), c);
                };
              if (l === a) return this.set(a, this.$M + t);
              if (l === u) return this.set(u, this.$y + t);
              if ("day" === l) return d(1);
              if (l === s) return d(7);
              var h =
                  (((o = {})[r] = 6e4), (o[i] = 36e5), (o[n] = 1e3), o)[l] || 1,
                f = this.$d.getTime() + t * h;
              return v.w(f, this);
            }),
            (f.subtract = function (t, e) {
              return this.add(-1 * t, e);
            }),
            (f.format = function (t) {
              var e = this,
                n = this.$locale();
              if (!this.isValid()) return n.invalidDate || l;
              var r = t || "YYYY-MM-DDTHH:mm:ssZ",
                i = v.z(this),
                s = this.$H,
                a = this.$m,
                o = this.$M,
                u = n.weekdays,
                c = n.months,
                d = function (t, n, i, s) {
                  return (t && (t[n] || t(e, r))) || i[n].slice(0, s);
                },
                f = function (t) {
                  return v.s(s % 12 || 12, t, "0");
                },
                m =
                  n.meridiem ||
                  function (t, e, n) {
                    var r = t < 12 ? "AM" : "PM";
                    return n ? r.toLowerCase() : r;
                  },
                p = {
                  YY: String(this.$y).slice(-2),
                  YYYY: v.s(this.$y, 4, "0"),
                  M: o + 1,
                  MM: v.s(o + 1, 2, "0"),
                  MMM: d(n.monthsShort, o, c, 3),
                  MMMM: d(c, o),
                  D: this.$D,
                  DD: v.s(this.$D, 2, "0"),
                  d: String(this.$W),
                  dd: d(n.weekdaysMin, this.$W, u, 2),
                  ddd: d(n.weekdaysShort, this.$W, u, 3),
                  dddd: u[this.$W],
                  H: String(s),
                  HH: v.s(s, 2, "0"),
                  h: f(1),
                  hh: f(2),
                  a: m(s, a, !0),
                  A: m(s, a, !1),
                  m: String(a),
                  mm: v.s(a, 2, "0"),
                  s: String(this.$s),
                  ss: v.s(this.$s, 2, "0"),
                  SSS: v.s(this.$ms, 3, "0"),
                  Z: i,
                };
              return r.replace(h, function (t, e) {
                return e || p[t] || i.replace(":", "");
              });
            }),
            (f.utcOffset = function () {
              return -(15 * Math.round(this.$d.getTimezoneOffset() / 15));
            }),
            (f.diff = function (t, e, c) {
              var l,
                d = v.p(e),
                h = y(t),
                f = (h.utcOffset() - this.utcOffset()) * 6e4,
                m = this - h,
                p = v.m(this, h);
              return (
                (p =
                  (((l = {})[u] = p / 12),
                  (l[a] = p),
                  (l[o] = p / 3),
                  (l[s] = (m - f) / 6048e5),
                  (l.day = (m - f) / 864e5),
                  (l[i] = m / 36e5),
                  (l[r] = m / 6e4),
                  (l[n] = m / 1e3),
                  l)[d] || m),
                c ? p : v.a(p)
              );
            }),
            (f.daysInMonth = function () {
              return this.endOf(a).$D;
            }),
            (f.$locale = function () {
              return p[this.$L];
            }),
            (f.locale = function (t, e) {
              if (!t) return this.$L;
              var n = this.clone(),
                r = g(t, e, !0);
              return r && (n.$L = r), n;
            }),
            (f.clone = function () {
              return v.w(this.$d, this);
            }),
            (f.toDate = function () {
              return new Date(this.valueOf());
            }),
            (f.toJSON = function () {
              return this.isValid() ? this.toISOString() : null;
            }),
            (f.toISOString = function () {
              return this.$d.toISOString();
            }),
            (f.toString = function () {
              return this.$d.toUTCString();
            }),
            t
          );
        })()).prototype),
        (y.prototype = w),
        [
          ["$ms", e],
          ["$s", n],
          ["$m", r],
          ["$H", i],
          ["$W", "day"],
          ["$M", a],
          ["$y", u],
          ["$D", c],
        ].forEach(function (t) {
          w[t[1]] = function (e) {
            return this.$g(e, t[0], t[1]);
          };
        }),
        (y.extend = function (t, e) {
          return t.$i || (t(e, D, y), (t.$i = !0)), y;
        }),
        (y.locale = g),
        (y.isDayjs = $),
        (y.unix = function (t) {
          return y(1e3 * t);
        }),
        (y.en = p[m]),
        (y.Ls = p),
        (y.p = {}),
        y);
    },
    6230: function (t) {
      t.exports = "object" == typeof self ? self.FormData : window.FormData;
    },
    8557: function (t, e, n) {
      (window.__NEXT_P = window.__NEXT_P || []).push([
        "/dashboard/calendar/create",
        function () {
          return n(3704);
        },
      ]);
    },
    85825: function (t, e, n) {
      "use strict";
      var r = n(85893),
        i = n(17064);
      let s = (t) => {
        let {
          className: e,
          onClick: n,
          children: s,
          type: a,
          variant: o = "contained",
        } = t;
        return (0, r.jsx)(i.E.button, {
          whileTap: { y: 1.5 },
          onClick: n,
          type: a,
          className:
            "rounded-md capitalize p-2  min-w-[100px] hover:bg-[#009ABC] "
              .concat(
                "outlined" === o
                  ? "bg-transparent text-textColor hover:bg-slate-100 border-2"
                  : "text-white bg-highlight",
                " "
              )
              .concat(e),
          children: s,
        });
      };
      e.Z = s;
    },
    99537: function (t, e, n) {
      "use strict";
      var r = n(85893);
      let i = (t) => {
        let { className: e, children: n } = t;
        return (0, r.jsx)("div", {
          className: "bg-[#ffffff] rounded-md shadow-sm w-full p-4 ".concat(e),
          children: n,
        });
      };
      e.Z = i;
    },
    29186: function (t, e, n) {
      "use strict";
      var r = n(85893);
      let i = (t) => {
        let {
          label: e,
          type: n,
          value: i,
          name: s,
          onChange: a,
          error: o,
          errorMessage: u,
          id: c,
        } = t;
        return (0, r.jsxs)("div", {
          children: [
            (0, r.jsxs)("div", {
              className: "relative  group min-w-xs ",
              children: [
                (0, r.jsx)("label", {
                  htmlFor: "",
                  className:
                    "absolute left-3  top-1/2\n        -translate-y-1/2 \n        group-focus-within:bg-white\n        group-focus-within:translate-y-[-200%]\n        group-focus-within:px-1 \n        group-focus-within:text-highlight transition-transform pointer-events-none\n         leading-none\n        group-focus-within:scale-90   duration-300 text-[#BDC0CC] "
                      .concat(
                        i
                          ? "translate-y-[-200%] text-highlight  scale-90 px-1 bg-white"
                          : "",
                        " "
                      )
                      .concat(o ? " text-red-400" : ""),
                  children: e,
                }),
                (0, r.jsx)("input", {
                  type: n,
                  value: i,
                  name: s,
                  className:
                    "border-[1px] border-slate-300 rounded-md w-full p-2 outline-highlight hover:border-slate-800  bg-transparent focus:border-highlight focus:outline-none ".concat(
                      o ? "border border-red-400" : ""
                    ),
                  onChange: a,
                  autoComplete: "new-password",
                  id: c,
                }),
              ],
            }),
            o
              ? (0, r.jsx)("p", {
                  className: "text-xs text-red-400 mt-1",
                  children: u,
                })
              : null,
          ],
        });
      };
      e.Z = i;
    },
    3704: function (t, e, n) {
      "use strict";
      n.r(e),
        n.d(e, {
          default: function () {
            return v;
          },
        });
      var r = n(85893),
        i = n(85825),
        s = n(29186),
        a = n(9198),
        o = n.n(a),
        u = n(99537),
        c = n(53641),
        l = n(48228),
        d = n(67294),
        h = n(25192),
        f = n(6230),
        m = n.n(f),
        p = n(27484),
        $ = n.n(p);
      let g = () => {
          let [t, e] = (0, d.useState)({
              summary: "",
              location: "",
              description: "",
              startDateTime: new Date(),
              endDateTime: "",
              AttendeeEmail: "",
              popopReminder: "",
            }),
            n = new (m())(),
            r = (0, l.D)(
              () => (
                n.append("Summary", t.summary),
                n.append("Description", t.description),
                n.append("AttendeeEmail[0]", t.AttendeeEmail),
                n.append(
                  "StartDateTime",
                  $()(t.startDateTime.toISOString()).format("YYYY-MM-DDTHH:mm")
                ),
                n.append("Location", "Lagos"),
                n.append(
                  "EndDateTime",
                  $()(t.startDateTime.toISOString()).format("YYYY-MM-DDTHH:mm")
                ),
                n.append("EmailReminderTime", "2"),
                n.append("PopUpReminderTime", "50"),
                c.d.request({
                  url: "",
                  method: "POST",
                  data: n,
                  headers: { "Content-Type": "multipart/form-data" },
                })
              ),
              {
                onSuccess: () => {
                  (0, h.C)("Done");
                },
              }
            ),
            i = (t) =>
              e(
                (e) => (
                  console.log(t.toISOString()), { ...e, startDateTime: t }
                )
              );
          return {
            calendarDetails: t,
            handleChange: function (t) {
              let n = t.target.name,
                r = t.target.value;
              e((t) => ({ ...t, [n]: r }));
            },
            handleDateChange: i,
            handleSubmit: function () {
              r.mutate();
            },
          };
        },
        y = () => {
          let {
            handleChange: t,
            calendarDetails: e,
            handleDateChange: n,
            handleSubmit: a,
          } = g();
          return (0, r.jsx)(r.Fragment, {
            children: (0, r.jsxs)(u.Z, {
              className: " p-8 mt-8",
              children: [
                (0, r.jsxs)("form", {
                  action: "",
                  className: "flex flex-col md:flex-row items-center",
                  children: [
                    (0, r.jsxs)("div", {
                      className: "w-full p-5 space-y-6",
                      children: [
                        (0, r.jsx)(s.Z, {
                          label: "Summary",
                          name: "summary",
                          onChange: t,
                          value: e.summary,
                        }),
                        (0, r.jsx)(s.Z, {
                          label: "Description",
                          name: "description",
                          onChange: t,
                          value: e.description,
                        }),
                        (0, r.jsx)(s.Z, {
                          label: "Email",
                          name: "AttendeeEmail",
                          onChange: t,
                          value: e.AttendeeEmail,
                        }),
                      ],
                    }),
                    (0, r.jsx)("div", {
                      className: "w-full full-display grid place-items-center",
                      children: (0, r.jsx)(o(), {
                        selected: e.startDateTime,
                        onChange: (t) => n(t),
                        inline: !0,
                        showTimeSelect: !0,
                      }),
                    }),
                  ],
                }),
                (0, r.jsxs)("div", {
                  className: " flex gap-8 justify-center",
                  children: [
                    (0, r.jsx)(i.Z, {
                      variant: "outlined",
                      children: "Cancel",
                    }),
                    (0, r.jsx)(i.Z, { onClick: a, children: "Upload" }),
                  ],
                }),
              ],
            }),
          });
        };
      var v = y;
    },
    25192: function (t, e, n) {
      "use strict";
      n.d(e, {
        C: function () {
          return c;
        },
      });
      var r = n(85893),
        i = n(86455),
        s = n.n(i),
        a = n(77630),
        o = n.n(a);
      let u = o()(s()),
        c = (t) =>
          u.fire({
            title: (0, r.jsx)("strong", { children: "Successful" }),
            html: (0, r.jsx)("i", { children: t }),
            icon: "success",
          });
    },
  },
  function (t) {
    t.O(0, [711, 198, 774, 888, 179], function () {
      return t((t.s = 8557));
    }),
      (_N_E = t.O());
  },
]);
