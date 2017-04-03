import { PagerOption } from "app/Models/Common/PagerOption";

export interface ResultJson {
	/**
	 * 错误代码
	 */
	errorCode: number;

	/**
	 * 消息
	 */
	msg: string;

	/**
	 * 数据
	 */
	data: any;

	/**
	 * 分页信息
	 */
	pageOption: PagerOption;

	/**
	 * 返回时间
	 */
	dateTime: Date;

}
