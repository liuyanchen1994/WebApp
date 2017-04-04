
import { Model } from 'app/Models/Model';

export interface BingNewsListView {

	/**
	 * Guid
	 */
    id: string;

	/**
	 * 状态值
	 */
    status: number;

	/**
	 * 创建时间
	 */
    createdTime: Date;

	/**
	 * 更新时间
	 */
    updatedTime: Date;


	/**
	 * 标题
	 */
    title: string;

	/**
	 * 内容概要
	 */
    description: string;

	/**
	 * 来源地址
	 */
    url: string;

	/**
	 * 缩略图链接
	 */
    thumbnailUrl: string;

	/**
	 * 标签
	 */
    tags: string;

	/**
	 * 来源
	 */
    provider: string;

}

