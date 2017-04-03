export interface PagerOption {

	/**
	 * 当前页  必传
	 */
    currentPage: number;

	/**
	 * 总条数  必传
	 */
    total: number;

	/**
	 * 分页记录数（每页条数 默认每页15条）
	 */
    pageSize: number;

	/**
	 * 路由地址(格式如：/Controller/Action) 默认自动获取
	 */
    routeUrl: string;

	/**
	 * 样式 默认 bootstrap样式 1
	 */
    styleNum: number;


}