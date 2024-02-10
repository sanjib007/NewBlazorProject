//const colors = require('tailwindcss/colors');
module.exports = {
    theme: {
        extend: {
            colors: {
                clifford: '#da373d',
                deep_blue: '#034EA2',
                bd_color: '#F5F6FA',
                user_bg: '#0CABFF',
                main_bg: '#f0f2f8',
                db_text_color: '#344058',
                my_blue: '#006FFF',
                my_green: '#06E244',
                my_dark: '#343A40',
                my_gray: '#8F8F8F',
                light_gray: '#D9D9D9',
                active_bg: '#f5f6fa',
                most_deep_blue: '#034792',
            },
            screens: {
                /*xs: '480px',*/
                xs: '375px',
                mobile: '375px',
            },
        }
    },
    purge: {
        enabled: true,
        content: [
            './**/*.html',
            './**/*.razor'
        ],
    },
    darkMode: false,
    variants: {
        extend: {},
    },

    plugins: [],
}