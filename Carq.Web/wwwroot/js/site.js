const carModel = {
    el: '#app',
    data() {
        return {
            report: {},
            rto: undefined,
            service: undefined,
            regn:''
        }
    },
    created() {},
    mounted() {},
    computed: {},
    methods: {
        fetchReport() {
            if (this.regn.length < 13) return;
            fetch('/api/report/'+this.regn)
                .then(response => response.json())
                .then(data => {
                    this.report = data;
                    this.rto = JSON.parse(data.rtoDetails).data;
                    this.service = JSON.parse(data.serviceDetails).data.result;
                });
        },
        regnUpd(e) {
            var val = e.target.value.replaceAll('-', '');
            var pieces = ['', '', '', ''];
            pieces[0] = val.substring(0, 2).toUpperCase();
            if (val.length > 2) pieces[1] = val.substring(2, 4);
            if (val.length > 4) pieces[2] = val.substring(4, 6).toUpperCase();
            if (val.length > 6) pieces[3] = val.substring(6, 10);
            val = pieces.filter(x => x.length > 0).join('-');
            e.target.value = val;
        },
    },
};
