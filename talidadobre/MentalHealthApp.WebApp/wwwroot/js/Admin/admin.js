

var UserEdit = Vue.extend({
    template: '#product-edit',
    data: function () {
        return { product: findProduct(this.$route.params.product_id) };
    },
    methods: {
        updateProduct: function () {
            var product = this.$get('product');
            products[findProductKey(product.id)] = {
                id: product.id,
                name: product.name,
                description: product.description,
                price: product.price
            };
            router.go('/');
        }
    }
});
