<template>
    <div>
        <label>
            Upload photos:
            <input type="file" id="files" ref="files" multiple v-on:change="handleFilesUpload()" />
        </label>
        <button v-on:click="submitFile()">Send</button>
    </div>
</template>

<script>
    import Axios from 'axios';
    import Config from '@/config';
    import '@/cookies';
    import Utils from '@/utils';

    export default {
        data() {
            return {
                files: []
            }
        },
        methods: {
            handleFilesUpload() {
                this.files = this.$refs.files.files;
            },
            submitFile() {
                var uploadFunc = function (filesBase64) {
                    Axios({
                        method: 'post',
                        url: Config.uploadPhotosApiPath,
                        data: { login: self.$cookies.get('login'), token: self.$cookies.get('token'), files: filesBase64 }
                    }).then(response => {
                        if (response.data.success) {
                            alert('OK');
                        }
                    });
                };
                Utils.filesToBase64(this.files, uploadFunc);
            }
        }
    }
</script>

<style scoped>
</style>
