<template>
  <el-main>
    <div class="flex">
      <el-input v-model="search" size="mini" placeholder="请输入关键字检索"></el-input>
      <el-button type="primary" size="mini" @click="flush()">查询</el-button>
      <div class="flex1"></div>
      <el-button type="success" size="mini" icon="el-icon-circle-plus" @click="AddApiResource()">创建</el-button>
      <el-button type="danger" size="mini" icon="el-icon-delete" @click="DeleteApiResource()">删除</el-button>
    </div>
    <el-table
      :data="ApiresourceData"
      ref="multipleTable"
      tooltip-effect="dark"
      size="mini"
      border
      align="center"
      style="width:100%"
      :stripe="true"
      @selection-change="handleSelectionChange"
    >
      <el-table-column type="selection" prop="id" label="序号"></el-table-column>
      <el-table-column prop="name" label="名称"></el-table-column>
      <el-table-column prop="description" label="描述"></el-table-column>
      <el-table-column prop="created" label="创建时间"></el-table-column>
      <el-table-column label="操作" align="center" width="350px">
        <template slot-scope="scope">
          <el-button type="primary" size="mini" @click="edit(scope.row)">编辑</el-button>
          <el-button type="primary" size="mini" @click="jump('ApiScopes',scope.row)">API作用域</el-button>
          <el-button type="primary" size="mini" @click="jump('ApiSecret',scope.row)">API密钥</el-button>
          <el-button type="primary" size="mini" @click="jump('ApiProperties',scope.row)">API属性</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination
      class="page_footer_box"
      @current-change="flush()"
      @size-change="flush()"
      background
      layout="total, sizes, prev, pager, next, jumper"
      :total="totalCount"
      :current-page.sync="currentPage"
      :page-sizes="[1,10,30,50]"
      :page-size.sync="pageSize"
    ></el-pagination>
    <apiresource-Edit :config="config" @flush="flush"></apiresource-Edit>
  </el-main>
</template>
<script>
import apiresourceEdit from "../components/modules/ApiResourceEdit";

export default {
  components: {
    apiresourceEdit
  },
  data() {
    return {
      multipleSelection: [],
      ApiresourceData: [],
      totalCount: 0,
      pageSize: 10,
      currentPage: 1,
      search: "",
      config: {
        show: false,
        title: "",
        data: {}
      }
    };
  },
  async mounted() {
    await this.flush();
  },
  methods: {
    async edit(row) {
      if (row.id) {
        let apidata = await this.$http.get("api/ApiResource/GetApiResource", {
          id: row.id
        });
        this.config.title = "修改ApiResource";
        this.config.data = apidata;
        this.config.type = "API";
        this.config.show = true;
      }
    },
    jump(path, row) {
      this.$router.push({ name: path, query: { apiresourceId: row.id } });
    },
    DeleteApiResource() {
      if (this.multipleSelection && this.multipleSelection.length) {
        const ids = this.multipleSelection.map(item => item.id);
        this.$confirm("确认删除所选用户?", "删除", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning"
        })
          .then(async () => {
            for (let index = 0; index < ids.length; index++) {
              let result = await this.$http.post(
                "api/ApiResource/RemoveApiResource",
                ids[index]
              );
              if (result) {
                this.$message({
                  showClose: true,
                  type: "success",
                  message: "删除成功!"
                });
              }
            }
            this.flush();
          })
          .catch(() => {
            this.$message({
              showClose: true,
              type: "info",
              message: "已取消删除!"
            });
          });
      } else {
        this.$message({
          showClose: true,
          type: "warning",
          message: "请选择要删除的行"
        });
      }
    },
    handleSelectionChange(val) {
      this.multipleSelection = val;
    },
    AddApiResource() {
      this.config.title = "添加ApiResource";
      this.config.data = {};
      this.config.show = true;
      this.config.type = "API";
    },
    async flush() {
      let result = await this.$http.get("api/ApiResource/GetApiResources", {
        Search: this.search,
        isdesc: true,
        Page: this.currentPage,
        PageSize: this.pageSize
      });
      this.totalCount = result.totalCount;
      this.ApiresourceData = result.data;
    }
  }
};
</script>
